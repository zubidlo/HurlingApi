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
    [RoutePrefix("api/players")]
    public class PlayersController : ApiController
    {
        private readonly IRepository _repository = new FantasyHurlingRepository();
        private readonly PlayerDTOFactory _factory = new PlayerDTOFactory();

        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PlayerDTO>> GetPlayers()
        {
            IEnumerable<Player> players = await _repository.Players().GetAllAsync();
            IEnumerable<PlayerDTO> playerDTOs = _factory.GetDTOCollection(players);
            IQueryable<PlayerDTO> oDataPlayerDTOs = playerDTOs.AsQueryable<PlayerDTO>();
            return oDataPlayerDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPlayerById(int id)
        {
            Player player;

            //try to get requested player
            try { player = await _repository.Players().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (player == null)
            {
                return new NotFoundActionResult(Request, "Could not find player id=" + id + "."); 
            }

            PlayerDTO playerDTO = _factory.GetDTO(player);
            //send ok response
            return Ok(playerDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditPlayer([FromUri] int id, [FromBody] PlayerDTO playerDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != playerDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match the Id from " + 
                                    "request body: " + playerDTO.Id + "!");
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Player player;

            //try to get requested player
            try { player = await _repository.Players().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (player == null)
            {
                return new NotFoundActionResult(Request, "Could not find player id=" + id + "."); 
            }

            //position can't be changed
            if (player.PositionId != playerDTO.PositionId)
            {
                return new ConflictActionResult(Request, "Changing this player position could break repository rules. " +
                    "There is only one player per position allowed in a team. Ask Martin.");
            }

            //if lastweekpoints are modified update overallpoints
            if (playerDTO.LastWeekPoints != player.LastWeekPoints)
            {
                playerDTO.OverallPoints += playerDTO.LastWeekPoints;

                //get all teams this player is in
                List<Team> teams = (List<Team>) await _repository.Teams().GetAllAsync();
                foreach(var team in teams)
                {
                    List<Player> players = team.Players.AsQueryable<Player>().ToList();
                    Player playerInTeam = players.Find(p => p.Id == id);
                    if (playerInTeam != null) 
                    { 
                        team.OverAllPoints += playerDTO.OverallPoints;
                        team.LastWeekPoints += playerDTO.LastWeekPoints;
                    }
                }
            }

            //now playerDTO is ok, set player's properties
            player.FirstName = playerDTO.FirstName;
            player.LastName = playerDTO.LastName;
            player.GaaTeam = playerDTO.GaaTeam;
            player.LastWeekPoints = playerDTO.LastWeekPoints;
            player.OverallPoints = playerDTO.OverallPoints;
            player.Price = playerDTO.Price;
            player.Rating = playerDTO.Rating;
            player.Injured = playerDTO.Injured;
            player.PositionId = playerDTO.PositionId;


            //try to update repository
            try { int result = await _repository.Players().UpdateAsync(player); }
            catch (Exception) { throw; }

            //send no content response
            return Ok("Player Id=" + id + " was successfully updated.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostPlayer([FromBody] PlayerDTO playerDTO)
        {
            //if the model state is not valit send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if a position with given id exists in the repository
            bool exist = await _repository.Positions().ExistAsync(p => p.Id == playerDTO.PositionId);

            //if doesn't exists send bad request response
            if (!exist)
            {
                return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
            }

            //playerDTO is ok, make new player
            Player player = _factory.GeTModel(playerDTO);
            player.Id = 0;
            player.LastWeekPoints = 0;
            player.OverallPoints = 0;

            //try to insert player into repository
            try { int result = await _repository.Players().InsertAsync(player); }
            catch (Exception) { throw; }

            //InsertAsync(player) created new id, so playerDTO must reflect that
            playerDTO = _factory.GetDTO(player);

            //send created at route response
            return Created<PlayerDTO>(Request.RequestUri + "/id/" + playerDTO.Id.ToString(), playerDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePlayer([FromUri] int id)
        {
            Player player;

            //try to get a player with given id
            try { player = await _repository.Players().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (player == null)
            {
                return new NotFoundActionResult(Request, "Could not find player id=" + id + ".");
            }

            PlayerDTO playerDTO = _factory.GetDTO(player);

            //try to delete the player
            try { int result = await _repository.Players().RemoveAsync(player); }
            catch (Exception)
            {
                return new ConflictActionResult(Request, "Deleting player with Id=" + id + " would break referential integrity " +
                                    "of the repository. You must first manualy delete this player from all the " + 
                                    "teams he is in. Ask Martin to add automate cascade deleting feature.");
            }

            //send ok response
            return Ok(playerDTO);
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