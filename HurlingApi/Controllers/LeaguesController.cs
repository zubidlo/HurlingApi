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

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<LeagueDTO>> GetLeagues()
        {
            //find all leagues in the repository
            var leagues = await _leaguesRepository.GetAllAsync();
            return _factory.GetCollection(leagues).AsQueryable<LeagueDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(LeagueDTO))]
        public async Task<IHttpActionResult> GetLeagueById(int id)
        {
            try
            {
                //find a league with given id
                var league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if one found in the repository
                if (league == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(league));
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
            //check if id from URI is the same as id from request body
            if (leagueDTO.Id != id)
            {
                return BadRequest("League Id from request URI: " + id + " doesn't match team Id from request body: " + leagueDTO.Id + "!");
            }

            //check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find a league with given id
                var league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if the league exists in the repository
                if (league == null)
                {
                    return BadRequest("League with id=" + id + "doesn't exist in the repository.");
                }

                //leagueDTO is valid, let's update the update the repository
                league.Name = leagueDTO.Name;
                league.NextFixtures = leagueDTO.NextFixtures;
                league.Week = leagueDTO.Week;

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
                var leagues = await _leaguesRepository.GetAllAsync();

                //check if there is more then zero
                if (leagues.Count() > 0)
                {
                    return BadRequest("There is already a league in the repository. We allow only one league to exist.");
                }

                //the leagueDTO is valid, let's insert it into repository
                var league = _factory.GeTModel(leagueDTO);
                await _leaguesRepository.InsertAsync(league);
                return CreatedAtRoute("DefaultRoute", new { id = leagueDTO.Id }, _factory.GetDTO(league));

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
                //find a league with given id
                var league = await _leaguesRepository.FindAsync(l => l.Id == id);

                //check if the league exists in the repository
                if (league == null)
                {
                    return BadRequest("League with id=" + id + "doesn't exist in the repository.");
                }

                try
                {
                    //find a team references this league
                    var team = await _teamsRepository.FindAsync(t => t.LeagueId == id);

                    //check if found
                    if (team != null)
                    {
                        return BadRequest("Can't delete this league, because there is still one team which still have a reference to it.");
                    }
                }
                catch (InvalidOperationException)
                {
                    return BadRequest("Can't delete this league, because there are still some teams which still have a reference to it.");
                }
                
                //everything is checked, so let's remove the league
                await _leaguesRepository.RemoveAsync(league);
                return Ok(_factory.GetDTO(league));
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }
    }
}