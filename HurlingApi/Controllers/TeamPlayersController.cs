using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HurlingApi.Models;

namespace HurlingApi.Controllers
{
    public class TeamPlayersController : ApiController
    {
        private HurlingModelContext db = new HurlingModelContext();

        // GET: api/TeamPlayers
        public IQueryable<TeamPlayer> GetTeamPlayers()
        {
            return db.TeamPlayers;
        }

        // GET: api/TeamPlayers/5
        [ResponseType(typeof(TeamPlayer))]
        public async Task<IHttpActionResult> GetTeamPlayer(int id)
        {
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            return Ok(teamPlayer);
        }

        // PUT: api/TeamPlayers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeamPlayer(int id, TeamPlayer teamPlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teamPlayer.Id)
            {
                return BadRequest();
            }

            db.Entry(teamPlayer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamPlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TeamPlayers
        [ResponseType(typeof(TeamPlayer))]
        public async Task<IHttpActionResult> PostTeamPlayer(TeamPlayer teamPlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamPlayers.Add(teamPlayer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = teamPlayer.Id }, teamPlayer);
        }

        // DELETE: api/TeamPlayers/5
        [ResponseType(typeof(TeamPlayer))]
        public async Task<IHttpActionResult> DeleteTeamPlayer(int id)
        {
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            db.TeamPlayers.Remove(teamPlayer);
            await db.SaveChangesAsync();

            return Ok(teamPlayer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamPlayerExists(int id)
        {
            return db.TeamPlayers.Count(e => e.Id == id) > 0;
        }
    }
}