﻿using System;
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
    public class LeaguesController : ApiController
    {
        private HurlingModelContext db = new HurlingModelContext();

        // GET: api/Leagues
        public IQueryable<League> GetLeagues()
        {
            return db.Leagues;
        }

        // GET: api/Leagues/5
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> GetLeague(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            return Ok(league);
        }

        // PUT: api/Leagues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLeague(int id, League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != league.Id)
            {
                return BadRequest();
            }

            db.Entry(league).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
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

        // POST: api/Leagues
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> PostLeague(League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Leagues.Add(league);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = league.Id }, league);
        }

        // DELETE: api/Leagues/5
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> DeleteLeague(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            db.Leagues.Remove(league);
            await db.SaveChangesAsync();

            return Ok(league);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeagueExists(int id)
        {
            return db.Leagues.Count(e => e.Id == id) > 0;
        }
    }
}