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
        private readonly Repositiory<Player> _playersRepository = new Repositiory<Player>(new HurlingModelContext());
        private readonly Repositiory<Position> _positionsRepository = new Repositiory<Position>(new HurlingModelContext());
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
            try
            {
                //get requested player
                Player player = await _playersRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (player == null)
                {
                    return NotFound();
                }

                PlayerDTO playerDTO = _factory.GetDTO(player);
                return Ok(playerDTO);
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
        public async Task<IHttpActionResult> EditPlayer([FromUri] int id, [FromBody] PlayerDTO playerDTO)
        {
            //check if id from URI matches Id from request body
            if (id != playerDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match the Id from request body: " + playerDTO.Id + "!");
            }

            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get requested player
                Player player = await _playersRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (player == null)
                {
                    return NotFound();
                }

                //get a position the player is referencing
                Position position = await _positionsRepository.FindAsync(p => p.Id == playerDTO.PositionId);

                //check if exists
                if (position == null)
                {
                    return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
                }

                //playerDTO is ok, update the player
                player.FirstName = playerDTO.FirstName;
                player.LastName = playerDTO.LastName;
                player.GaaTeam = playerDTO.GaaTeam;
                player.LastWeekPoints = playerDTO.LastWeekPoints;
                player.OverallPoints = playerDTO.OverallPoints;
                player.Price = playerDTO.Price;
                player.Rating = playerDTO.Rating;
                player.Injured = playerDTO.Injured;
                player.PositionId = playerDTO.PositionId;

                //player must be the reference to actual player in the repository. UpdateAsync will throw exception otherwise.
                //I can't just UpdateAsync(new Player());
                await _playersRepository.UpdateAsync(player);
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
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> PostPlayer([FromBody] PlayerDTO playerDTO)
        {
            //check if the model state is valit
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                //get a position the player is referencing
                Position position = await _positionsRepository.FindAsync(p => p.Id == playerDTO.PositionId);

                //check if exists
                if (position == null)
                {
                    return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
                }

                //playerDTO is ok, insert the player
                Player player = _factory.GeTModel(playerDTO);
                await _playersRepository.InsertAsync(player);
                
                //InsertAsync(player) created new id, so playerDTO must reflect that
                playerDTO = _factory.GetDTO(player);
               
                return CreatedAtRoute("DefaultRoute", new { id = player.Id }, playerDTO);
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> DeletePlayer([FromUri] int id)
        {
            try
            {
                //get requested player
                var player = await _playersRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (player == null)
                {
                    return NotFound();
                }

                //everything is ok, delete the player
                await _playersRepository.RemoveAsync(player);
                PlayerDTO playerDTO = _factory.GetDTO(player);
                return Ok(playerDTO);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Deleting player with Id=" + id + " would break referential integrity of the repository. Check PlayersInTeams entity for references.");
            }
        }
    }
}