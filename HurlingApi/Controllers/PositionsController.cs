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
    /// <summary></summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/positions")]
    public class PositionsController : ApiController
    {
        private readonly Repositiory<Position> _repository = new Repositiory<Position>(new HurlingModelContext());
        private readonly PositionFactoryDTO _factory = new PositionFactoryDTO();

        /// <summary></summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PositionDTO>> GetPostions()
        {
            var positions = await _repository.GetAllAsync();
            return _factory.GetCollection(positions).AsQueryable<PositionDTO>();
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionById(int id)
        {
            try
            {
                var position = await _repository.FindAsync(u => u.Id == id);
                if (position == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(position));
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionByName([FromUri] string name)
        {
            Position position = null;
            try
            {
                position = await _repository.FindAsync(p => p.Name == name);
                if (position == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(position));
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="positionDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditPosition([FromUri] int id, [FromBody] PositionDTO positionDTO)
        {
            if (positionDTO.Id != id)
            {
                return BadRequest("Position Id from request URI: " + id + " doesn't match position Id from request body: " + positionDTO.Id + "!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Position position, position1;
            try
            {
                position = await _repository.FindAsync(p => p.Id == id);
                if (position == null)
                {
                    return BadRequest("The position with Id = " + id + " doesn't exists.");
                }

                position1 = await _repository.FindAsync(p => p.Name == positionDTO.Name);
                if ((position1 != null) && (position1.Id != positionDTO.Id) && (position1.Name == positionDTO.Name))
                {
                    return BadRequest("There is already a position with Name:" + positionDTO.Name + " in the repository!");
                }

                position.Name = positionDTO.Name;
                await _repository.UpdateAsync(position);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}