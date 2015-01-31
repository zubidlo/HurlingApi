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
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/teams")]
    public class TeamsController : ApiController
    {
        private readonly IRepository _repository = new FantasyHurlingRepository();
        private readonly TeamDTOFactory _teamFactory = new TeamDTOFactory();
        private readonly PlayerDTOFactory _playerFactory = new PlayerDTOFactory();
       
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<TeamDTO>> GetTeams()
        {
            IEnumerable<Team> teams = await _repository.Teams().GetAllAsync();
            IEnumerable<TeamDTO> teamDTOs = _teamFactory.GetDTOCollection(teams);
            IQueryable <TeamDTO> oDataTeamDTOs = teamDTOs.AsQueryable();
            return oDataTeamDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamById([FromUri] int id)
        {
            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null)
            {
                return new NotFoundActionResult(Request, "Could not find team id=" + id + ".");
            }

            var teamDTO = _teamFactory.GetDTO(team);
            //send ok response
            return Ok(teamDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("name/{name}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamByName([FromUri] string name)
        {
            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Name == name); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null)
            {
                return new NotFoundActionResult(Request, "Could not find team name=" + name + ".");
            }

            var teamDTO = _teamFactory.GetDTO(team);
            //send ok response
            return Ok(teamDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}/players")]
        [HttpGet]
        public async Task<IQueryable<PlayerDTO>> GetTeamPlayers([FromUri] int id)
        {
            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null)
            {
                throw new ObjectNotFoundException("Could not find team Id=" + team.Id + ".");
            }

            IEnumerable<PlayerDTO> playerDTOs = _playerFactory.GetDTOCollection(team.Players);
            IQueryable<PlayerDTO> oDataPlayerDTOs = playerDTOs.AsQueryable();
            return oDataPlayerDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teamDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditTeam([FromUri] int id, [FromBody] TeamDTO teamDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != teamDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match " +
                                "the Id from request body: " + teamDTO.Id + "!");
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (team == null)
            {
                return new NotFoundActionResult(Request, "Could not find team Id=" + id + ".");
            }

            Team team1;

            //try to get a team with same name
            try { team1 = await _repository.Teams().FindSingleAsync(t => t.Name == teamDTO.Name); }
            catch (InvalidOperationException) { throw; }

            //if exists and if it is different that one we are editing throw bad request response
            if (team1 != null && team1.Id != id)
            {
                return new ConflictActionResult(Request, "There is already a team with name:" + teamDTO.Name + " in " +
                                "the repository! We allow only unique team names.");
            }

            //teams can't be moved between users
            if (teamDTO.UserId != team.UserId)
            {
                return new ConflictActionResult(Request, "You cannot change UserID. It's not allowed to move " +
                                                        "teams between users. Ask Martin.");
            }

            bool exist;

            //find out if the league referenced by this team LeagueId exists
            exist = await _repository.Leagues().ExistAsync(l => l.Id == teamDTO.LeagueId);

            //if doesn't exist send bad request response
            if (!exist)
            {
                return new NotFoundActionResult(Request, "Could not find league Id=" + teamDTO.LeagueId + ".");
            }

            //update the team's properties
            team.Name = teamDTO.Name;
            team.OverAllPoints = teamDTO.OverAllPoints;
            team.LastWeekPoints = teamDTO.LastWeekPoints;
            team.Budget = teamDTO.Budget;
            team.LeagueId = teamDTO.LeagueId;
            team.UserId = teamDTO.UserId;

            //try to update the team in the repository
            try { int result = await _repository.Teams().UpdateAsync(team); }
            catch (Exception) { throw; }

            //send no content response
            return Ok("Team Id=" + id + " was successfully updated.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [Route("id/{teamId:int}/player/id/{playerId:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PostTeamPlayer([FromUri] int teamId, [FromUri] int playerId)
        {
            Team team;
            Player player;

            //try to get requested team and player
            try
            {
                team = await _repository.Teams().FindSingleAsync(t => t.Id == teamId);
                player = await _repository.Players().FindSingleAsync(p => p.Id == playerId);
            }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null) 
            {
                return new NotFoundActionResult(Request, "Could not find team Id=" + teamId + ".");
            }
            
            if (player == null)
            {
                return new NotFoundActionResult(Request, "Could not find player Id=" + playerId + ".");
            }

            //find out if there is this player already in the team
            bool playerAlreadyInThisTeam = team.Players.Any(p => p.Id == playerId);

            if (playerAlreadyInThisTeam)
            {
                return new ConflictActionResult(Request, "Player with id=" + playerId + " is in this team already!");
            }

            //find out if there is a player with same field position (we allow only one player per position)
            bool positionAlreadyInThisTeam = team.Players.Any(p => p.PositionId == player.PositionId);

            if (positionAlreadyInThisTeam)
            {
                return new ConflictActionResult(Request, "There is already a player with the same position in this team!"); 
            }

            //find out if team has bugdet to buy this player
            if (team.Budget < player.Price)
            {
                return new ConflictActionResult(Request, "Your this team budget=" + team.Budget + " is not big enough " +
                                                " to cover this player price=" + player.Price + ".");
            }

            //decrease the team budget
            team.Budget -= player.Price;

            //add player to this team
            team.Players.Add(player);

            //try to save changes in the repository
            try { int result = await _repository.Teams().SaveChangesAsync(); }
            catch (Exception) { throw; }

            //send ok response
            return Ok("Player with Id=" + playerId + " was added to team Id=" + teamId + ".");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostTeam([FromBody] TeamDTO teamDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if a team with same name exist
            bool exist = await _repository.Teams().ExistAsync(t => t.Name == teamDTO.Name);
                
            //if exist send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "There is already an team with name:" + teamDTO.Name + " in " +
                                    "the repository!. We allow only unique team names.");
            }

            //find out if user the team is referencing exists
            exist = await _repository.Users().ExistAsync(u => u.Id == teamDTO.UserId);

            //check if doesn't exist send bad request response
            if (!exist)
            {
                return new NotFoundActionResult(Request, "Could not find user with Id=" + teamDTO.UserId + ".");
            }

            //find out if team with same userId exists
            exist = await _repository.Teams().ExistAsync(t => t.UserId == teamDTO.UserId);

            //if exists send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "User with id=" + teamDTO.UserId + " already has " +
                                "a team! We allow only one team per user.");
            }

            //find out if the league this team is referencing exist
            exist = await _repository.Leagues().ExistAsync(l => l.Id == teamDTO.LeagueId);

            //check if doesn't exist send bad request response
            if (!exist)
            {
                return new NotFoundActionResult(Request, "Could not find league Id=" + teamDTO.LeagueId + ".");
            }

            var team = _teamFactory.GeTModel(teamDTO);

            //new team points to 0
            team.OverAllPoints = 0;
            team.LastWeekPoints = 0;

            //try to insert the team into the repository
            try { int result = await _repository.Teams().InsertAsync(team); }
            catch (Exception) { throw; }

            //InsertAsync(team) created new id, so teamDTO must reflect that
            teamDTO = _teamFactory.GetDTO(team);

            //send created at route response
            return Created<TeamDTO>(Request.RequestUri + "/id/" + teamDTO.Id.ToString(), teamDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTeam([FromUri] int id)
        {
            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exist send not found response
            if (team == null)
            {
                return new NotFoundActionResult(Request, "Could not find team Id=" + id + "."); 
            }

            //try to remove the team from the repository
            try { int result = await _repository.Teams().RemoveAsync(team); }
            catch (Exception)
            {
                return new ConflictActionResult(Request, "Deleting team with Id=" + id + " would break referential " +
                                "integrity of the repository. First manualy remove all players " +
                                "from this team. Ask Martin for automatic cascade removal functionality.");
            }   
             
            //send ok response
            return Ok("Team Id=" + id + " deleted.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [Route("id/{teamId:int}/player/id/{playerId:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePlayerFromTeam([FromUri] int teamId, [FromUri] int playerId)
        {
            Team team;

            //try to get requested team
            try { team = await _repository.Teams().FindSingleAsync(t => t.Id == teamId); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (team == null)
            {
                return new NotFoundActionResult(Request, "Could not find team Id=" + teamId + "."); 
            }

            var player = team.Players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
            {
                return new NotFoundActionResult(Request, "Could not find player Id=" + playerId + ".");
            }

            //return money back to team budget
            team.Budget += player.Price;

            //remove the player from the team
            team.Players.Remove(player);
            
            //try to save changes in the repository
            try { int result = await _repository.Teams().SaveChangesAsync(); }
            catch (Exception) { throw; }

            //send ok response
            return Ok("Player Id=" + playerId + " was removed from team Id=" + teamId + ".");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}