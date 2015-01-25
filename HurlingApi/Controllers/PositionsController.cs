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
        private readonly Repositiory<Position> _positionsRepository =
            new Repositiory<Position>(new HurlingModelContext());
        private readonly Repositiory<Player> _playersRepository =
            new Repositiory<Player>(new HurlingModelContext());
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
            Position position;

            //try to get requested position
            try { position = await _positionsRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (position == null) { return NotFound(); }

            PositionDTO positionDTO = _factory.GetDTO(position);
            //send ok response
            return Ok(positionDTO);
            
        }

        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> GetPositionByName([FromUri] string name)
        {
            Position position;

            //try to get requested position
            try { position = await _positionsRepository.FindSingleAsync(p => p.Name == name); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (position == null) { return NotFound(); }

            PositionDTO positionDTO = _factory.GetDTO(position);
            //send ok response
            return Ok(positionDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditPosition([FromUri] int id, [FromBody] PositionDTO positionDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != positionDTO.Id) 
            { 
                return BadRequest("The id from URI: " + id + " doesn'singleItem match " +
                                "the Id from request body: " + positionDTO.Id + "!"); 
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Position position, position1;

            //try to get requested position
            try { position = await _positionsRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }
                
            //if doesn't exists send not found response
            if (position == null) { return NotFound(); }
                
            //try to get a position with same name
            try { position1 = await _positionsRepository.FindSingleAsync(p => p.Name == positionDTO.Name); }
            catch (InvalidOperationException) { throw; }
                
            //if that exist and if it is different that one we are editing send bad request response
            if (position1 != null && position1.Id != id) 
            { 
                return BadRequest("There is already a position with Name:" + positionDTO.Name + " in " +
                                    "the repository! We allow only unique position names.");
            }

            //positionDTO is ok, update the position
            position.Name = positionDTO.Name;

            //try to update the position in the repository
            try { int result = await _positionsRepository.UpdateAsync(position); }
            catch (Exception) { throw; }

            //send no content response
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> PostPosition([FromBody] PositionDTO positionDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Position position;

            //try to get a position with same name
            try { position = await _positionsRepository.FindSingleAsync(p => p.Name == positionDTO.Name); }
            catch (InvalidOperationException) { throw; }
                
            //if exists send bad request response
            if (position != null) 
            {
                return BadRequest("There is already a position with Name:" + positionDTO.Name + " in " +
                                    "the repository! We allow only unique position names.");
            }

            position = _factory.GeTModel(positionDTO);

            //try to insert the position into the repository
            try { int result = await _positionsRepository.InsertAsync(position); }
            catch (Exception) { throw; }    

            //InsertAsync(position) created new id, so positionDTO must reflect that
            positionDTO = _factory.GetDTO(position);

            //send created at route response
            return CreatedAtRoute("DefaultRoute", new { id = position.Id }, positionDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(PositionDTO))]
        public async Task<IHttpActionResult> DeletePosition([FromUri] int id)
        {
            Position position;

            //try to get requested position
            try { position = await _positionsRepository.FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exists send not found response
            if (position == null) { return NotFound(); }

            //check if any player references this position
            bool exist = await _playersRepository.ExistAsync(p => p.PositionId == id);

            //if exists send bad request response
            if (exist)
            {
                return BadRequest("Can't delete this position, because there are " +
                                "still some players referencing the position!");
            }

            //try to remove the position from the repository
            try {int result = await _positionsRepository.RemoveAsync(position); }
            catch (Exception) { throw; }    
            
            PositionDTO positionDTO = _factory.GetDTO(position);
            
            //send ok response
            return Ok(positionDTO);
        }
    }
}