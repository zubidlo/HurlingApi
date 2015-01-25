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
        private readonly Repositiory<League> _leaguesRepository = new Repositiory<League>(new HurlingModelContext());
        private readonly Repositiory<Team> _teamsRepository = new Repositiory<Team>(new HurlingModelContext());
        private readonly LeagueDTOFactory _factory = new LeagueDTOFactory();
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
            try
            {
                //get requested league
                League league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if exists
                if (league == null)
                {
                    return NotFound();
                }

                LeagueDTO leagueDTO = _factory.GetDTO(league);
                return Ok();
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
        public async Task<IHttpActionResult> EditLeague([FromUri] int id, [FromBody] LeagueDTO leagueDTO)
        {
            //check if id from URI match Id from request body
            if (id != leagueDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match the Id from request body: " + leagueDTO.Id + "!");
            }

            //check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get requested league
                League league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if exists
                if (league == null)
                {
                    return NotFound();
                }

                //leagueDTO is ok, update the league
                league.Name = leagueDTO.Name;
                league.NextFixtures = leagueDTO.NextFixtures;
                league.Week = leagueDTO.Week;

                //league must be the reference to actual league in the repository. UpdateAsync will throw exception otherwise.
                //I can't just UpdateAsync(new League());
                await _leaguesRepository.UpdateAsync(league);
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
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> PostLeague([FromBody] LeagueDTO leagueDTO)
        {

            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get all leagues
                IEnumerable<League> leagues = await _leaguesRepository.GetAllAsync();

                //check if there is more then zero
                if (leagues.Count() > 0)
                {
                    return BadRequest("There is already a league in the repository! We allow only one league to exist.");
                }

                //leagueDTO is ok, insert the league
                League league = _factory.GeTModel(leagueDTO);
                await _leaguesRepository.InsertAsync(league);

                //InsertAsync(league) created new id, so leagueDTO must reflect that
                leagueDTO = _factory.GetDTO(league);
                return CreatedAtRoute("DefaultRoute", new { id = league.Id }, leagueDTO);

            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> DeleteLeague([FromUri] int id)
        {
            try
            {
                //get requested league
                League league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if exists
                if (league == null)
                {
                    return NotFound();
                }

                try
                {
                    //find a team referencing this league
                    Team team = await _teamsRepository.FindAsync(t => t.LeagueId == id);

                    //check if exists
                    if (team != null)
                    {
                        return BadRequest("Can't delete this league, because team id=" + team.Id + " still referencing the league!");
                    }
                }
                catch (InvalidOperationException)
                {
                    return BadRequest("Can't delete this league, because there are still some teams referencing the league!");
                }

                //everything is ok, let's remove the league
                await _leaguesRepository.RemoveAsync(league);
                LeagueDTO leagueDTO = _factory.GetDTO(league);
                return Ok(leagueDTO);
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }
    }
}