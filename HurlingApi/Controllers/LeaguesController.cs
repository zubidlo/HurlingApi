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
    [RoutePrefix("api/leagues")]
    public class LeaguesController : ApiController
    {
        private readonly IRepository _repository = new FantasyHurlingRepository();
        private readonly LeagueDTOFactory _factory =  new LeagueDTOFactory();
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<LeagueDTO>> GetLeagues()
        {
            IEnumerable<League> leagues = await _repository.Leagues().GetAllAsync();
            IEnumerable<LeagueDTO> leagueDTOs = _factory.GetDTOCollection(leagues);
            IQueryable<LeagueDTO> oDataLeagueDTOs = leagueDTOs.AsQueryable<LeagueDTO>();
            return oDataLeagueDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLeagueById(int id)
        {
            League league;

            //try to get requested league
            try { league = await _repository.Leagues().FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }
           
            //if doesn't exists send not found response
            if (league == null)
            {
                return new NotFoundActionResult(Request, "Could not find league id=" + id + ".");
            }

            var leagueDTO = _factory.GetDTO(league);

            //send ok response
            return Ok(leagueDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leagueDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditLeague([FromUri] int id, [FromBody] LeagueDTO leagueDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != leagueDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match the" + 
                                    " Id from request body: " + leagueDTO.Id + "!");
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            League league;

            //try to get requested league
            try { league = await _repository.Leagues().FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exist send not found response
            if (league == null)
            { 
                return new NotFoundActionResult(Request, "Could not find league id=" + id + "."); 
            }

            //leagueDTO is ok, update the league's properties
            league.Name = leagueDTO.Name;
            league.NextFixtures = leagueDTO.NextFixtures;
            league.Week = leagueDTO.Week;

            //try to update repository
            try { int result = await _repository.Leagues().UpdateAsync(league); }
            catch (Exception) { throw; }

            //send no content response
            return Ok("League Id=" + id + " was successfully updated.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leagueDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostLeague([FromBody] LeagueDTO leagueDTO)
        {

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //get all leagues
            IEnumerable<League> leagues = await _repository.Leagues().GetAllAsync();

            //if there is a league in the repository already send bad request response
            if (leagues.Count() > 0)
            {
                return new ConflictActionResult(Request, "There is already a league in the repository!" +
                                    " We allow only one league to exist.");
            }

            var league = _factory.GeTModel(leagueDTO);

            //try to insert the league into the repository
            try { int result = await _repository.Leagues().InsertAsync(league); }
            catch (Exception) { throw; }

            //InsertAsync(league) created new id, so leagueDTO must reflect that
            leagueDTO = _factory.GetDTO(league);

            //send created at route response
            return Created<LeagueDTO>(Request.RequestUri + "/id/" + leagueDTO.Id.ToString() ,leagueDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLeague([FromUri] int id)
        {
            League league;

            //try to get requested league
            try { league = await _repository.Leagues().FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (league == null)
            {
                return new NotFoundActionResult(Request, "Could not find league id=" + id + ".");
            }

            //find out if any team referencing this league exists
            bool exist = await _repository.Teams().ExistAsync(t => t.LeagueId == id);

            //if exists send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "Can't delete league Id=" + id + ", because there are still " 
                                                + "some teams referencing it! Delete all the team in this league first.");
            }

            //try to remove the league from the repository
            try { int result = await _repository.Leagues().RemoveAsync(league); }
            catch (Exception) { throw; }

            //send ok response
            return Ok("League Id=" + id + " deleted.");
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