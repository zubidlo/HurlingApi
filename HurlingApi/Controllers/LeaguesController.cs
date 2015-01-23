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
    [RoutePrefix("api/leagues")]
    public class LeaguesController : ApiController
    {
        private readonly Repositiory<League> _repository = new Repositiory<League>(new HurlingModelContext());
        private readonly LeagueDTOFactory _factory = new LeagueDTOFactory();

        /// <summary></summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<LeagueDTO>> GetLeagues()
        {
            var leagues = await _repository.GetAllAsync();
            return _factory.GetCollection(leagues).AsQueryable<LeagueDTO>();
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> GetLeagueById(int id)
        {
            try
            {
                var league = await _repository.FindAsync(l => l.Id == id);
                if (league == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(league));
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one league with Id=" + id + " in the repository."));
            }
        }
    }
}