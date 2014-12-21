using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HurlingApi.Models;
using System.Threading.Tasks;

namespace HurlingApi.Controllers
{
    public class PositionsController : ApiController
    {
        private HurlingModelContext db = new HurlingModelContext();
        private DTOFactory factory = new DTOFactory();

        // GET: api/Positions
        
        public IQueryable<PositionDTO> GetAllPositions()
        {
            var posList = factory.GetAllPositionDTOs(db.Positions.Include(pos => pos.Players).ToList());
            var positionsQueryable = posList.AsQueryable<PositionDTO>();
            
            return positionsQueryable;
        }

        // GET: api/Positions/5
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPosition(int id)
        {
            Position position = await db.Positions.Include(p => p.Players).Where(pos => pos.Id == id).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }

            return Ok(factory.GetPositionDTO(position));
        }

        // PUT: api/Positions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPosition(int id, Position position)
        {
            Console.WriteLine(id);
            Console.WriteLine(position);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.Id)
            {
                return BadRequest(id + " != " + position.Id);
            }

            db.Entry(position).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
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

        // POST: api/Positions
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> PostPosition(Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Positions.Add(position);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = position.Id }, position);
        }

        // DELETE: api/Positions/5
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> DeletePosition(int id)
        {
            Position position = await db.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            db.Positions.Remove(position);
            await db.SaveChangesAsync();

            return Ok(position);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return db.Positions.Count(e => e.Id == id) > 0;
        }
    }
}