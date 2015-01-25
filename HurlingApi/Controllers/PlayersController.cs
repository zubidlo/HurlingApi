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
    [RoutePrefix("api/players")]
    public class PlayersController : ApiController
    {
        private readonly Repositiory<Player> _playersRepository =
            new Repositiory<Player>(new HurlingModelContext());
        private readonly Repositiory<Position> _positionsRepository =
            new Repositiory<Position>(new HurlingModelContext());
        private readonly PlayerDTOFactory _factory = new PlayerDTOFactory();
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed) 
            { 
                if (disposing)
                {
                    _playersRepository.Dispose();
                    _positionsRepository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PlayerDTO>> GetPlayers()
        {
            IEnumerable<Player> players = await _playersRepository.GetAllAsync();
            IEnumerable<PlayerDTO> playerDTOs = _factory.GetDTOCollection(players);
            return playerDTOs.AsQueryable<PlayerDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> GetPlayerById(int id)
        {
            Player player;

            //try to get requested player
            try { player = await _playersRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (player == null) { return NotFound(); }

            PlayerDTO playerDTO = _factory.GetDTO(player);
            //send ok response
            return Ok(playerDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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
            try { player = await _playersRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (player == null) { return NotFound(); }

            //find out if position with given id exists in the repository
            bool exist = await _positionsRepository.ExistAsync(p => p.Id == playerDTO.PositionId);

            //if doesn't exists send bad request response
            if (!exist)
            {
                return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
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
            try { int result = await _playersRepository.UpdateAsync(player); }
            catch (Exception) { throw; }

            //send no content response
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> PostPlayer([FromBody] PlayerDTO playerDTO)
        {
            //if the model state is not valit send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if a position with given id exists in the repository
            bool exist = await _positionsRepository.ExistAsync(p => p.Id == playerDTO.PositionId);

            //if doesn't exists send bad request response
            if (!exist)
            {
                return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
            }

            //playerDTO is ok, make new player
            Player player = _factory.GeTModel(playerDTO);

            //try to insert player into repository
            try { int result = await _playersRepository.InsertAsync(player); }
            catch (Exception) { throw; }

            //InsertAsync(player) created new id, so playerDTO must reflect that
            playerDTO = _factory.GetDTO(player);

            //send created at route response
            return CreatedAtRoute("DefaultRoute", new { id = player.Id }, playerDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> DeletePlayer([FromUri] int id)
        {
            Player player;

            //try to get a player with given id
            try { player = await _playersRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (player == null) { return NotFound(); }

            //try to delete the player
            try { int result = await _playersRepository.RemoveAsync(player); }
            catch (Exception)
            {
                return BadRequest("Deleting player with Id=" + id + " would break referential integrity " +
                                    "of the repository. Check PlayersInTeams entity for references.");
            }

            PlayerDTO playerDTO = _factory.GetDTO(player);
            //send ok response
            return Ok(playerDTO);
        }
    }
}