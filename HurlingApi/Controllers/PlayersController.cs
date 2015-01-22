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
    /// <summary></summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/players")]
    public class PlayersController : ApiController
    {
        private readonly Repositiory<Player> _repository = new Repositiory<Player>(new HurlingModelContext());
        private readonly PlayerDTOFactory _factory = new PlayerDTOFactory();

        /// <summary></summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PlayerDTO>> GetPlayers()
        {
            var players = await _repository.GetAllAsync();
            return _factory.GetCollection(players).AsQueryable<PlayerDTO>();
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> GetPlayerById(int id)
        {
            try
            {
                var player = await _repository.FindAsync(p => p.Id == id);
                if (player == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(player));
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one player with Id=" + id + " in the repository."));
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditPlayer([FromUri] int id, [FromBody] PlayerDTO playerDTO)
        {
            if (playerDTO.Id != id)
            {
                return BadRequest("Player Id from request URI: " + id + " doesn't match player Id from request body: " + playerDTO.Id + "!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.UpdateAsync(_factory.GeTModel(playerDTO));
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> PostPlayer([FromBody] PlayerDTO playerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPlayer = _factory.GeTModel(playerDTO);

            try
            {
                await _repository.InsertAsync(newPlayer);
                return CreatedAtRoute("DefaultRoute", new { id = playerDTO.Id }, _factory.GetDTO(newPlayer));
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(PlayerDTO))]
        public async Task<IHttpActionResult> DeletePlayer([FromUri] int id)
        {
            try
            {
                var player = await _repository.FindAsync(p => p.Id == id);
                if (player == null)
                {
                    return NotFound();
                }
                await _repository.RemoveAsync(player);
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
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}