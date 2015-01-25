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

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PlayerDTO>> GetPlayers()
        {
            //find all players
            var players = await _playersRepository.GetAllAsync();
            return _factory.GetCollection(players).AsQueryable<PlayerDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> GetPlayerById(int id)
        {
            try
            {
                //find a player with given id
                var player = await _playersRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (player == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(player));
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
            //check if id from URI is the same as id from request body
            if (playerDTO.Id != id)
            {
                return BadRequest("Player Id from request URI: " + id + " doesn't match player Id from request body: " + playerDTO.Id + "!");
            }

            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find a position the player is referencing
                var position = await _positionsRepository.FindAsync(p => p.Id == playerDTO.PositionId);

                //check if exists
                if (position == null)
                {
                    return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
                }

                //playerDTO is ok, let's update the position
                await _playersRepository.UpdateAsync(_factory.GeTModel(playerDTO));
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
                //find a position the player is referencing
                var position = await _positionsRepository.FindAsync(p => p.Id == playerDTO.PositionId);

                //check if exists
                if (position == null)
                {
                    return BadRequest("Postion with Id=" + playerDTO.PositionId + " doesn't exist in the repository.");
                }

                //playerDTO is ok, let's insert it
                var player = _factory.GeTModel(playerDTO);
                await _playersRepository.InsertAsync(player);
                return CreatedAtRoute("DefaultRoute", new { id = playerDTO.Id }, _factory.GetDTO(player));
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
                //find a player with given id
                var player = await _playersRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (player == null)
                {
                    return BadRequest("Player with id=" + id + "doesn't exist in the repository.");
                }

                //everything is ok, let's delete the player
                await _playersRepository.RemoveAsync(player);
                return Ok(_factory.GetDTO(player));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("Deleting player with Id=" + id + " would break referential integrity of the repository. Check the relations between this player and other entities.");
            }
        }

        /// <summary></summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _playersRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}