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
    }
}