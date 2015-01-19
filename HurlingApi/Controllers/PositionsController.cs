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
using System.Web.Http.Cors;

namespace HurlingApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/positions")]
    public class PositionsController : ApiController
    {
        private readonly HurlingModelContext _context = new HurlingModelContext();
        private readonly Repositiory<Position> _repository = new Repositiory<Position>();
        private readonly PositionFactoryDTO _factory = new PositionFactoryDTO();

        /// <summary>
        /// Gets all positions.
        /// This method supports OData filters, for example:
        /// /api/positions?$orderby=Name : will return positions sorted by username
        /// </summary>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PositionDTO>> GetPostions()
        {
            var positions = await _repository.GetAllAsync();
            var positionDTOs = _factory.GetCollection(positions).AsQueryable<PositionDTO>();
            return positionDTOs;
        }

        /// <summary>
        /// Gets position by his Id.
        /// </summary>
        /// <param name="id">The Id of the user.</param>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPosition(int id)
        {
            var position = await _repository.FindAsync(u => u.Id == id);

            if (position == null)
            {
                return NotFound();
            }

            var positionDTO = _factory.GetDTO(position);
            return Ok(positionDTO);
        }

        /// <summary>
        /// Gets position by it's Name
        /// </summary>
        /// <param name="username">The Name of the position.</param>
        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string name)
        {
            Position position = null;
            try
            {
                position = await _repository.FindAsync(p => p.Name == name);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("There is more than one positon with name:" + name + " in the repository...");
            }

            if (position == null)
            {
                return NotFound();
            }

            var positionDTO = _factory.GetDTO(position);
            return Ok(positionDTO);
        }

        // PUT: api/Positions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPosition(int id, Position position)
        {
            throw new NotImplementedException();
        }

        // POST: api/Positions
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> PostPosition(Position position)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Positions/5
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> DeletePosition(int id)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Count(p => p.Id == id) > 0;
        }
    }
}