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

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<TeamDTO>> GetTeams()
        {
            //find all teams
            var teams = await _teamsRepository.GetAllAsync();
            return _factory.GetCollection(teams).AsQueryable<TeamDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(TeamDTO))]
        public async Task<IHttpActionResult> GetTeamById(int id)
        {
            try
            {
                //find a team with given id
                var team = await _teamsRepository.FindAsync(t => t.Id == id);

                //check if there is no team with given id
                if (team == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(team));
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
                //find a team with given name
                var team = await _teamsRepository.FindAsync(t => t.Name == name);

                //check if there is no team with given name
                if (team == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(team));
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
            //check if id from URL and id from request body are the same
            if (teamDTO.Id != id)
            {
                return BadRequest("The team Id from request URI: " + id + " doesn't match team Id from request body: " + teamDTO.Id + "!");
            }

            //check if model is ok
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find a team with given id (team to edit)
                var team = await _teamsRepository.FindAsync(t => t.Id == id);

                //check if there is one
                if (team == null)
                {
                    return BadRequest("Team with id=" + id + "doesn't exist in the repository.");
                }

                //find some other team with same name (which is not allowed)
                var team1 = await _teamsRepository.FindAsync(t => t.Name == teamDTO.Name);

                //check if there is some other team with the same name as edited team (which is not allowed)
                if (team1 != null && team1.Id != teamDTO.Id)
                {
                    return BadRequest("There is already an team with name:" + teamDTO.Name + " in the repository!");
                }

                //find a user with given id
                var user = await _usersRepository.FindAsync(u => u.Id == teamDTO.UserId);

                //check if there is one
                if (user == null)
                {
                    return BadRequest("The user with Id=" + teamDTO.UserId + " doesn't exist in the repository");
                }

                //find some other team with the same user id
                team1 = await _teamsRepository.FindAsync(t => t.UserId == teamDTO.UserId);

                //check if found team is different from edited one (we allow only one team per user)
                if (team1 != null && team1.Id != teamDTO.Id)
                {
                    return BadRequest("User with Id=" + teamDTO.UserId + " already has a team. We allow only one team per user.");
                }

                //find league with given id
                var league = await _leaguesRepository.FindAsync(l => l.Id == teamDTO.LeagueId);

                //check if there is one
                if (league == null)
                {
                    return BadRequest("The league with Id=" + teamDTO.LeagueId + " doesn't exist in the repository.");
                }

                //teamDTO is valid let's update the team
                team.Name = teamDTO.Name;
                team.OverAllPoints = teamDTO.OverAllPoints;
                team.LastWeekPoints = teamDTO.LastWeekPoints;
                team.Budget = teamDTO.Budget;
                team.LeagueId = teamDTO.LeagueId;
                team.UserId = teamDTO.UserId;

                await _teamsRepository.UpdateAsync(team);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException)
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
                //find a team with the same name as our new team
                var team = await _teamsRepository.FindAsync(t => t.Name == teamDTO.Name);

                //check if there is already a team with given name
                if (team != null)
                {
                    return BadRequest("There is already an team with name:" + teamDTO.Name + " in the repository.");
                }

                //find user with given id
                var user = await _usersRepository.FindAsync(u => u.Id == teamDTO.UserId);

                //check if user with given id exists
                if (user == null)
                {
                    return BadRequest("User with Id=" + teamDTO.UserId + " doesn't exist.");
                }

                //find a team with same userId as our new team
                team = await _teamsRepository.FindAsync(t => t.UserId == teamDTO.UserId);

                //check if team exists
                if (team != null)
                {
                    return BadRequest("User with id=" + teamDTO.UserId + " already has a team. We allow only one team per user.");
                }

                //find league with given id
                var league = await _leaguesRepository.FindAsync(l => l.Id == teamDTO.LeagueId);

                //check if league with given id exists
                if (league == null)
                {
                    return BadRequest("League with Id=" + teamDTO.LeagueId + " doesn't exist in the repository.");
                }

                //teamDTO is valid let's insert the team into repository
                team = _factory.GeTModel(teamDTO);
                await _teamsRepository.InsertAsync(team);
                return CreatedAtRoute("DefaultRoute", new { id = teamDTO.Id }, _factory.GetDTO(team));
            }
            catch (InvalidOperationException)
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
                //find a team with given id
                var team = await _teamsRepository.FindAsync(t => t.Id == id);

                //check if the team exists
                if (team == null)
                {
                    return BadRequest("Team with id=" + id + "doesn't exist in the repository.");
                }

                //team exists, let's delete it
                await _teamsRepository.RemoveAsync(team);
                return Ok(_factory.GetDTO(team));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("Deleting team with Id=" + id + " would break referential integrity of the repository. There must be still some references to this team in the repository. PlayersInTeams maybe?");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _teamsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}