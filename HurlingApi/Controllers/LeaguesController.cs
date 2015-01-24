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

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="leagueDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditLeague([FromUri] int id, [FromBody] LeagueDTO leagueDTO)
        {
            if (leagueDTO.Id != id)
            {
                return BadRequest("League Id from request URI: " + id + " doesn't match league Id from request body: " + leagueDTO.Id + "!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            League league;

            try
            {
                league = await _repository.FindAsync(l => l.Id == id);
                if (league == null)
                {
                    return NotFound();
                }

                var league1 = await _repository.FindAsync(l => l.Name == leagueDTO.Name);
                if ((league1 != null) && (league1.Id != leagueDTO.Id) && (league1.Name == leagueDTO.Name))
                {
                    return BadRequest("There is already an league with Name:" + leagueDTO.Name + " in the repository!");
                }
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one league with Name=" + leagueDTO.Name + " in the repository."));
            }

            league.Name = leagueDTO.Name;
            league.NextFixtures = leagueDTO.NextFixtures;
            league.Week = leagueDTO.Week;

            try
            {
                await _repository.UpdateAsync(league);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="leagueDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> PostLeague([FromBody] LeagueDTO leagueDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var league = await _repository.FindAsync(l => l.Name == leagueDTO.Name);
                if (league != null)
                {
                    return BadRequest("There is already an league with Name:" + leagueDTO.Name + " in the repository.");
                }
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one league with Name=" + leagueDTO.Name));
            }

            var newLeague = _factory.GeTModel(leagueDTO);
            try
            {
                await _repository.InsertAsync(newLeague);
                return CreatedAtRoute("DefaultRoute", new { id = leagueDTO.Id }, _factory.GetDTO(newLeague));
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
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> DeleteLeague([FromUri] int id)
        {
            try
            {
                var league = await _repository.FindAsync(l => l.Id == id);
                if (league == null)
                {
                    return NotFound();
                }
                await _repository.RemoveAsync(league);
                return Ok(_factory.GetDTO(league));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("Deleting league with Id=" + id + " would break referential integrity of the repository. Check the relations between this league and other entities.");
            }
        }
    }
}