using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HurlingApi.Models;
using System.Web.Http.Cors;

namespace HurlingApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/teams")]
    public class TeamsController : ApiController
    {
        private readonly Repositiory<Team> _teamsRepository = new Repositiory<Team>(new HurlingModelContext());
        private readonly Repositiory<User> _usersRepository = new Repositiory<User>(new HurlingModelContext());
        private readonly Repositiory<League> _leaguesRepository = new Repositiory<League>(new HurlingModelContext());
        private readonly TeamDTOFactory _factory = new TeamDTOFactory();
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _teamsRepository.Dispose();
                    _usersRepository.Dispose();
                    _leaguesRepository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<TeamDTO>> GetTeams()
        {
            IEnumerable<Team> teams = await _teamsRepository.GetAllAsync();
            IEnumerable<TeamDTO> teamDTOs = _factory.GetDTOCollection(teams);
            return teamDTOs.AsQueryable<TeamDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> GetTeamById(int id)
        {
            try
            {
                //get requested team
                Team team = await _teamsRepository.FindSingleAsync(t => t.Id == id);

                //check if exists
                if (team == null)
                {
                    return NotFound();
                }

                TeamDTO teamDTO = _factory.GetDTO(team);
                return Ok(teamDTO);
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> GetTeamByName([FromUri] string name)
        {
            try
            {
                //find requested team
                Team team = await _teamsRepository.FindSingleAsync(t => t.Name == name);

                //check if exists
                if (team == null)
                {
                    return NotFound();
                }

                TeamDTO teamDTO = _factory.GetDTO(team);
                return Ok(teamDTO);
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditTeam([FromUri] int id, [FromBody] TeamDTO teamDTO)
        {
            //check if id from URI match Id from request body
            if (id != teamDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn'singleItem match the Id from request body: " + teamDTO.Id + "!");
            }

            //check if model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get requested team
                Team team = await _teamsRepository.FindSingleAsync(t => t.Id == id);

                //check if exists
                if (team == null)
                {
                    return NotFound();
                }

                //get a team with same name
                Team team1 = await _teamsRepository.FindSingleAsync(t => t.Name == teamDTO.Name);

                //check if exists and if it is different that one we are editing
                if (team1 != null && team1.Id != id)
                {
                    return BadRequest("There is already a team with name:" + teamDTO.Name + " in the repository! We allow only unique team names.");
                }

                //get the user this team is referencing
                User user = await _usersRepository.FindSingleAsync(u => u.Id == teamDTO.UserId);

                //check if exists
                if (user == null)
                {
                    return BadRequest("The user with Id=" + teamDTO.UserId + " doesn'singleItem exist in the repository!");
                }

                //get some other team with the same user id
                team1 = await _teamsRepository.FindSingleAsync(t => t.UserId == teamDTO.UserId);

                //check if found team is different from one we are editing
                if (team1 != null && team1.Id != id)
                {
                    return BadRequest("User with Id=" + teamDTO.UserId + " already has a team! We allow only one team per user.");
                }

                //get league with id this team is referencing
                League league = await _leaguesRepository.FindSingleAsync(l => l.Id == teamDTO.LeagueId);

                //check if exists
                if (league == null)
                {
                    return BadRequest("The league with Id=" + teamDTO.LeagueId + " doesn'singleItem exist in the repository!");
                }

                //teamDTO is ok, update the team
                team.Name = teamDTO.Name;
                team.OverAllPoints = teamDTO.OverAllPoints;
                team.LastWeekPoints = teamDTO.LastWeekPoints;
                team.Budget = teamDTO.Budget;
                team.LeagueId = teamDTO.LeagueId;
                team.UserId = teamDTO.UserId;

                //team must be the reference to actual team in the repository. UpdateAsync will throw exception otherwise.
                //I can'singleItem just UpdateAsync(new Team());
                await _teamsRepository.UpdateAsync(team);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                //internal server error
                throw;
            }
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> PostTeam([FromBody] TeamDTO teamDTO)
        {
            //check if model state is ok
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get a team with the same name
                Team team = await _teamsRepository.FindSingleAsync(t => t.Name == teamDTO.Name);

                //check if exists
                if (team != null)
                {
                    return BadRequest("There is already an team with name:" + teamDTO.Name + " in the repository!. We allow only unique team names.");
                }

                //get user the team is referencing
                User user = await _usersRepository.FindSingleAsync(u => u.Id == teamDTO.UserId);

                //check if exists
                if (user == null)
                {
                    return BadRequest("User with Id=" + teamDTO.UserId + " doesn'singleItem exist in the repository!");
                }

                //get a team with same userId as our new team
                team = await _teamsRepository.FindSingleAsync(t => t.UserId == teamDTO.UserId);

                //check if team exists
                if (team != null)
                {
                    return BadRequest("User with id=" + teamDTO.UserId + " already has a team! We allow only one team per user.");
                }

                //get the league this team is referencing
                League league = await _leaguesRepository.FindSingleAsync(l => l.Id == teamDTO.LeagueId);

                //check if exists
                if (league == null)
                {
                    return BadRequest("League with Id=" + teamDTO.LeagueId + " doesn'singleItem exist in the repository!");
                }

                //teamDTO is ok, insert the team
                team = _factory.GeTModel(teamDTO);
                await _teamsRepository.InsertAsync(team);

                //InsertAsync(team) created new id, so teamDTO must reflect that
                teamDTO = _factory.GetDTO(team);
                return CreatedAtRoute("DefaultRoute", new { id = team.Id }, teamDTO);
            }
            catch (Exception)
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> DeleteTeam([FromUri] int id)
        {
            try
            {
                //get requested team
                Team team = await _teamsRepository.FindSingleAsync(t => t.Id == id);

                //check if exists
                if (team == null)
                {
                    return NotFound();
                }

                //everything is ok, let's remove the team
                await _teamsRepository.RemoveAsync(team);
                TeamDTO teamDTO = _factory.GetDTO(team);
                return Ok(teamDTO);
            }
            catch (Exception)
            {
                return BadRequest("Deleting team with Id=" + id + " would break referential integrity of the repository. There must be still some references to this team in the repository. PlayersInTeams maybe?");
            }
        }
    }
}