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
    [RoutePrefix("api/leagues")]
    public class LeaguesController : ApiController
    {
        private readonly Repositiory<League> _leaguesRepository =
            new Repositiory<League>(new HurlingModelContext());
        private readonly Repositiory<Team> _teamsRepository =
            new Repositiory<Team>(new HurlingModelContext());
        private readonly LeagueDTOFactory _factory =
            new LeagueDTOFactory();
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _leaguesRepository.Dispose();
                    _teamsRepository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<LeagueDTO>> GetLeagues()
        {
            IEnumerable<League> leagues = await _leaguesRepository.GetAllAsync();
            IEnumerable<LeagueDTO> leagueDTOs = _factory.GetDTOCollection(leagues);
            return leagueDTOs.AsQueryable<LeagueDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> GetLeagueById(int id)
        {
            League league;

            //try to get requested league
            try { league = await _leaguesRepository.FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }
           
            //if doesn't exists send not found response
            if (league == null) { return NotFound(); }

            LeagueDTO leagueDTO = _factory.GetDTO(league);
            //send ok response
            return Ok(leagueDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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
            try { league = await _leaguesRepository.FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exist send not found response
            if (league == null) { return NotFound(); }

            //leagueDTO is ok, update the league's properties
            league.Name = leagueDTO.Name;
            league.NextFixtures = leagueDTO.NextFixtures;
            league.Week = leagueDTO.Week;

            //try to update repository
            try { int result = await _leaguesRepository.UpdateAsync(league); }
            catch (Exception) { throw; }

            //send no content response
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> PostLeague([FromBody] LeagueDTO leagueDTO)
        {

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //get all leagues
            IEnumerable<League> leagues = await _leaguesRepository.GetAllAsync();

            //if there is a league in the repository already send bad request response
            if (leagues.Count() > 0)
            {
                return BadRequest("There is already a league in the repository!" +
                                    " We allow only one league to exist.");
            }

            League league = _factory.GeTModel(leagueDTO);

            //try to insert the league into the repository
            try { int result = await _leaguesRepository.InsertAsync(league); }
            catch (Exception) { throw; }

            //InsertAsync(league) created new id, so leagueDTO must reflect that
            leagueDTO = _factory.GetDTO(league);

            //send created at route response
            return CreatedAtRoute("DefaultRoute", new { id = league.Id }, leagueDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> DeleteLeague([FromUri] int id)
        {
            League league;

            //try to get requested league
            try { league = await _leaguesRepository.FindSingleAsync(l => l.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (league == null) { return NotFound(); }

            //find out if any team referencing this league exists
            bool exist = await _teamsRepository.ExistAsync(t => t.LeagueId == id);

            //if exists send bad request response
            if (exist)
            { 
                return BadRequest("Can't delete this league, because there are still " 
                                    + "some teams referencing it!");
            }

            //try to remove the league from the repository
            try { int result = await _leaguesRepository.RemoveAsync(league); }
            catch (Exception) { throw; }

            LeagueDTO leagueDTO = _factory.GetDTO(league);
            //send ok response
            return Ok(leagueDTO);
        }
    }
}