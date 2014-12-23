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
using System.Reflection;

namespace HurlingApi.Controllers
{
    public class PositionsController : ApiController
    {
        private HurlingModelContext db = new HurlingModelContext();
        private DTOFactory factory = new DTOFactory();

        public PositionsController() { }

        // GET: api/Positions
        
        public IQueryable<PositionDTO> GetAllPositions()
        {
            var positionDTOs = factory.GetAllPositionDTOs(db.Positions).AsQueryable<PositionDTO>();
            return positionDTOs;
        }

        // GET: api/Positions/5
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPosition([FromUri] int id)
        {
            var position = await db.Positions.FindAsync(id);
            
            if (position == null)
            {
                return NotFound();
            }

            var positionDTO = factory.GetPositionDTO(position);
            return Ok(positionDTO);
        }

        // PUT: api/Positions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPosition([FromUri] int id, [FromBody] Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.Id)
            {
                return BadRequest("id from uri is not equal to id from body");
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
        public async Task<IHttpActionResult> PostPosition([FromBody] Position position)
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
        public async Task<IHttpActionResult> DeletePosition([FromUri] int id)
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