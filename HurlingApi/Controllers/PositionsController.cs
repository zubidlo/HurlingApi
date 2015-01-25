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
        private readonly Repositiory<Position> _positionsRepository = new Repositiory<Position>(new HurlingModelContext());
        private readonly Repositiory<Player> _playersRepository = new Repositiory<Player>(new HurlingModelContext());
        private readonly PositionDTOFactory _factory = new PositionDTOFactory();

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PositionDTO>> GetPostions()
        {
            //find all positions
            var positions = await _positionsRepository.GetAllAsync();
            return _factory.GetCollection(positions).AsQueryable<PositionDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionById(int id)
        {
            try
            {
                //find position with given id
                var position = await _positionsRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(position));
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionByName([FromUri] string name)
        {
            try
            {
                //find position with given name
                var position = await _positionsRepository.FindAsync(p => p.Name == name);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(position));
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
        public async Task<IHttpActionResult> EditPosition([FromUri] int id, [FromBody] PositionDTO positionDTO)
        {
            //check if id from URI match position if from request body
            if (positionDTO.Id != id)
            {
                return BadRequest("Position Id from request URI: " + id + " doesn't match position Id from request body: " + positionDTO.Id + "!");
            }

            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find position with given id
                var position = await _positionsRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                //find some other position with same name
                var position1 = await _positionsRepository.FindAsync(p => p.Name == positionDTO.Name);

                //check if found position is different that edited one
                if (position1 != null && position1.Id != positionDTO.Id)
                {
                    return BadRequest("There is already a position with Name:" + positionDTO.Name + " in the repository!");
                }

                //positionDTO is ok, let's update the position
                position.Name = positionDTO.Name;
                await _positionsRepository.UpdateAsync(position);
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
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> PostPosition([FromBody] PositionDTO positionDTO)
        {
            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find a position with the same name
                var position = await _positionsRepository.FindAsync(p => p.Name == positionDTO.Name);

                //check if exists
                if (position != null)
                {
                    return BadRequest("There is already a position with Name:" + positionDTO.Name + " in the repository.");
                }

                position = _factory.GeTModel(positionDTO);
                await _positionsRepository.InsertAsync(position);
                return CreatedAtRoute("DefaultRoute", new { id = positionDTO.Id }, _factory.GetDTO(position));
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> DeletePosition([FromUri] int id)
        {
            try
            {
                //find a position with given id
                var position = await _positionsRepository.FindAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                try
                {
                    //find player referencing this position
                    var player = await _playersRepository.FindAsync(p => p.PositionId == position.Id);

                    //check if exists
                    if (player != null)
                    {
                        return BadRequest("Can't delete this position, because there is still one player which still have a reference to it.");
                    }
                }
                catch (InvalidOperationException)
                {
                    return BadRequest("Can't delete this position, because there are still some players which still have a reference to it.");
                }

                //everything is ok, let's remove the position
                await _positionsRepository.RemoveAsync(position);
                return Ok(_factory.GetDTO(position));
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _positionsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}