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
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _positionsRepository.Dispose();
                    _playersRepository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PositionDTO>> GetPostions()
        {
            IEnumerable<Position> positions = await _positionsRepository.GetAllAsync();
            IEnumerable<PositionDTO> positionDTOs = _factory.GetDTOCollection(positions);
            return positionDTOs.AsQueryable<PositionDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionById(int id)
        {
            try
            {
                //get requested position
                Position position = await _positionsRepository.FindSingleAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                PositionDTO positionDTO = _factory.GetDTO(position);
                return Ok(positionDTO);
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
                //get requested position
                Position position = await _positionsRepository.FindSingleAsync(p => p.Name == name);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                PositionDTO positionDTO = _factory.GetDTO(position);
                return Ok(positionDTO);
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
            //check if id from URI matches Id from request body
            if (id != positionDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn'singleItem match the Id from request body: " + positionDTO.Id + "!");
            }

            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get requested position
                var position = await _positionsRepository.FindSingleAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                //get a position with same name
                var position1 = await _positionsRepository.FindSingleAsync(p => p.Name == positionDTO.Name);

                //check if exist and if it is different that one we are editing
                if (position1 != null && position1.Id != id)
                {
                    return BadRequest("There is already a position with Name:" + positionDTO.Name + " in the repository! We allow only unique positio names.");
                }

                //positionDTO is ok, update the position
                position.Name = positionDTO.Name;

                //position must be the reference to actual position in the repository. UpdateAsync will throw exception otherwise.
                //I can'singleItem just UpdateAsync(new Position());
                await _positionsRepository.UpdateAsync(position);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
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
                //get a position with same name
                Position position = await _positionsRepository.FindSingleAsync(p => p.Name == positionDTO.Name);

                //check if exists
                if (position != null)
                {
                    return BadRequest("There is already a position with Name:" + positionDTO.Name + " in the repository! We allow only unique position names.");
                }

                //positionDTO is ok, insert the position
                position = _factory.GeTModel(positionDTO);
                await _positionsRepository.InsertAsync(position);

                //InsertAsync(position) created new id, so positionDTO must reflect that
                positionDTO = _factory.GetDTO(position);

                return CreatedAtRoute("DefaultRoute", new { id = position.Id }, positionDTO);
            }
            catch (Exception)
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
                //get requested position
                Position position = await _positionsRepository.FindSingleAsync(p => p.Id == id);

                //check if exists
                if (position == null)
                {
                    return NotFound();
                }

                //check if any player references this position
                bool exist = await _playersRepository.ExistAsync(p => p.PositionId == id);

                //check if exists
                if (exist)
                {
                    return BadRequest("Can't delete this position, because there are still some players referencing the position!");
                }

                //everything is ok, let's remove the position
                await _positionsRepository.RemoveAsync(position);
                PositionDTO positionDTO = _factory.GetDTO(position);
                return Ok(positionDTO);
            }
            catch (Exception)
            {
                //internal server error
                throw;
            }
        }
    }
}