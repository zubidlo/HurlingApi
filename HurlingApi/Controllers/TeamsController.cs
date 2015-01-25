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
        private readonly Repositiory<Team> _teamsRepository =
            new Repositiory<Team>(new HurlingModelContext());
        private readonly Repositiory<User> _usersRepository =
            new Repositiory<User>(new HurlingModelContext());
        private readonly Repositiory<League> _leaguesRepository =
            new Repositiory<League>(new HurlingModelContext());
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
            IQueryable <TeamDTO> oDataTeamDTOs = teamDTOs.AsQueryable<TeamDTO>();
            return oDataTeamDTOs;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> GetTeamById(int id)
        {
            Team team;

            //try to get requested team
            try { team = await _teamsRepository.FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null) { return NotFound(); }

            TeamDTO teamDTO = _factory.GetDTO(team);

            //send ok response
            return Ok(teamDTO);
        }

        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> GetTeamByName([FromUri] string name)
        {
            Team team;

            //try to get requested team
            try { team = await _teamsRepository.FindSingleAsync(t => t.Name == name); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null) { return NotFound(); }

            TeamDTO teamDTO = _factory.GetDTO(team);

            //send ok response
            return Ok(teamDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditTeam([FromUri] int id, [FromBody] TeamDTO teamDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != teamDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn'singleItem match " +
                                "the Id from request body: " + teamDTO.Id + "!");
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Team team, team1;

            //try to get requested team
            try { team = await _teamsRepository.FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (team == null) { return NotFound(); }

            //try to get a team with same name
            try { team1 = await _teamsRepository.FindSingleAsync(t => t.Name == teamDTO.Name); }
            catch (InvalidOperationException) { throw; }

            //if exists and if it is different that one we are editing throw bad request response
            if (team1 != null && team1.Id != id)
            {
                return BadRequest("There is already a team with name:" + teamDTO.Name + " in " +
                                "the repository! We allow only unique team names.");
            }

            //find out if user referenced by this team UserId exists
           bool exist =  await _usersRepository.ExistAsync(u => u.Id == teamDTO.UserId);

            //if doesn't exist send bad request response
            if (!exist)
            {
                return BadRequest("The user with Id=" + teamDTO.UserId + " doesn't exist in the repository!");
            }

            //try to get some other team with the same user id
            try { team1 = await _teamsRepository.FindSingleAsync(t => t.UserId == teamDTO.UserId); }
            catch (InvalidOperationException) { throw; }

            //cif found team is different from one we are editing send bad request response
            if (team1 != null && team1.Id != id)
            {
                return BadRequest("User with Id=" + teamDTO.UserId + " already " +
                                "has a team! We allow only one team per user.");
            }

            //find out if the league referenced by this team UserId exists
            exist = await _leaguesRepository.ExistAsync(l => l.Id == teamDTO.LeagueId);

            //if doesn't exist send bad request response
            if (!exist)
            {
                return BadRequest("The league with Id=" + teamDTO.LeagueId + " doesn't exist in the repository!");
            }

            //update the team's properties
            team.Name = teamDTO.Name;
            team.OverAllPoints = teamDTO.OverAllPoints;
            team.LastWeekPoints = teamDTO.LastWeekPoints;
            team.Budget = teamDTO.Budget;
            team.LeagueId = teamDTO.LeagueId;
            team.UserId = teamDTO.UserId;

            //try to update the team in the repository
            try { int result = await _teamsRepository.UpdateAsync(team); }
            catch (Exception) { throw; }

            //send no content response
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> PostTeam([FromBody] TeamDTO teamDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if a team with same name exist
            bool exist = await _teamsRepository.ExistAsync(t => t.Name == teamDTO.Name);
                
            //if exist send bad request response
            if (exist)
            {
                return BadRequest("There is already an team with name:" + teamDTO.Name + " in " +
                                    "the repository!. We allow only unique team names.");
            }

            //find out if user the team is referencing exists
            exist = await _usersRepository.ExistAsync(u => u.Id == teamDTO.UserId);

            //check if doesn't exist send bad request response
            if (!exist)
            {
                return BadRequest("User with Id=" + teamDTO.UserId + " doesn't exist in the repository!");
            }

            //find out if team with same userId exists
            exist = await _teamsRepository.ExistAsync(t => t.UserId == teamDTO.UserId);

            //if exists send bad request response
            if (exist)
            {
                return BadRequest("User with id=" + teamDTO.UserId + " already has " +
                                "a team! We allow only one team per user.");
            }

            //find out if the league this team is referencing exist
            exist = await _leaguesRepository.ExistAsync(l => l.Id == teamDTO.LeagueId);

            //check if doesn't exist send bad request response
            if (!exist)
            {
                return BadRequest("League with Id=" + teamDTO.LeagueId + " doesn't exist in the repository!");
            }

            Team team = _factory.GeTModel(teamDTO);

            //try to insert the team into the repository
            try { int result = await _teamsRepository.InsertAsync(team); }
            catch (Exception) { throw; }

            //InsertAsync(team) created new id, so teamDTO must reflect that
            teamDTO = _factory.GetDTO(team);

            //send created at route response
            return CreatedAtRoute("DefaultRoute", new { id = team.Id }, teamDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> DeleteTeam([FromUri] int id)
        {
            Team team;

            //try to get requested team
            try { team = await _teamsRepository.FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exist send not found response
            if (team == null) { return NotFound(); }

            //try to remove the team from the repository
            try { int result = await _teamsRepository.RemoveAsync(team); }
            catch (Exception)
            {
                return BadRequest("Deleting team with Id=" + id + " would break referential " +
                                "integrity of the repository. There must be still some references " +
                                "to this team in the repository. PlayersInTeams maybe?");
            }   
             
            TeamDTO teamDTO = _factory.GetDTO(team);

            //send ok response
            return Ok(teamDTO);
        }
    }
}