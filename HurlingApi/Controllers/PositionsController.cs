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
        private IRepository _repository;
        private readonly PositionDTOFactory _factory = new PositionDTOFactory();
        private bool _disposed;

        public PositionsController() { _repository =  new FantasyHurlingRepository(); }

        public PositionsController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IQueryable<PositionDTO>> GetPostions()
        {
            IEnumerable<Position> positions = await _repository.Positions().GetAllAsync();
            IEnumerable<PositionDTO> positionDTOs = _factory.GetDTOCollection(positions);
            IQueryable<PositionDTO> oDataPositionDTOs = positionDTOs.AsQueryable<PositionDTO>();
            return oDataPositionDTOs;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPositionById(int id)
        {
            Position position;

            //try to get requested position
            try { position = await _repository.Positions().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (position == null)
            {
                return new NotFoundActionResult(Request, "Could not find position id=" + id + ".");
            }

            var positionDTO = _factory.GetDTO(position);
            //send ok response
            return Ok(positionDTO);
            
        }

        [Route("name/{name}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPositionByName([FromUri] string name)
        {
            Position position;

            //try to get requested position
            try { position = await _repository.Positions().FindSingleAsync(p => p.Name == name); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (position == null)
            {
                return new NotFoundActionResult(Request, "Could not find position name=" + name + ".");
            }

            var positionDTO = _factory.GetDTO(position);
            //send ok response
            return Ok(positionDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditPosition([FromUri] int id, [FromBody] PositionDTO positionDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != positionDTO.Id) 
            { 
                return BadRequest("The id from URI: " + id + " doesn't match " +
                                "the Id from request body: " + positionDTO.Id + "!"); 
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Position position;

            //try to get requested position
            try { position = await _repository.Positions().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }
                
            //if doesn't exists send not found response
            if (position == null) 
            {
                return new NotFoundActionResult(Request, "Could not find position id=" + id + ".");
            }
             
            Position position1;

            //try to get a position with same name
            try { position1 = await _repository.Positions().FindSingleAsync(p => p.Name == positionDTO.Name); }
            catch (InvalidOperationException) { throw; }
                
            //if that exist and if it is different that one we are editing send bad request response
            if (position1 != null && position1.Id != id) 
            {
                return new ConflictActionResult(Request, "There is already a position with Name:" + positionDTO.Name + " in " +
                                    "the repository! We allow only unique position names.");
            }

            //positionDTO is ok, update the position
            position.Name = positionDTO.Name;

            //try to update the position in the repository
            try { int result = await _repository.Positions().UpdateAsync(position); }
            catch (Exception) { throw; }

            //send no content response
            return Ok("Position Id=" + id + " was successfully updated.");
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostPosition([FromBody] PositionDTO positionDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Position position;

            //try to get a position with same name
            try { position = await _repository.Positions().FindSingleAsync(p => p.Name == positionDTO.Name); }
            catch (InvalidOperationException) { throw; }
                
            //if exists send bad request response
            if (position != null) 
            {
                return new ConflictActionResult(Request, "There is already a position with Name:" + positionDTO.Name + " in " +
                                    "the repository! We allow only unique position names.");
            }

            position = _factory.GeTModel(positionDTO);

            //try to insert the position into the repository
            try { int result = await _repository.Positions().InsertAsync(position); }
            catch (Exception) { throw; }    

            //InsertAsync(position) created new id, so positionDTO must reflect that
            positionDTO = _factory.GetDTO(position);

            //send created at route response
            return Created<PositionDTO>(Request.RequestUri + "/id/" + positionDTO.Id.ToString(), positionDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePosition([FromUri] int id)
        {
            Position position;

            //try to get requested position
            try { position = await _repository.Positions().FindSingleAsync(p => p.Id == id); }
            catch (InvalidOperationException) { throw; }
            
            //if doesn't exists send not found response
            if (position == null)
            {
                return new NotFoundActionResult(Request, "Could not find position id=" + id + ".");
            }

            //check if any player references this position
            bool exist = await _repository.Players().ExistAsync(p => p.PositionId == id);

            //if exists send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "Can't delete this position, because there are " +
                                "still some players referencing the position!");
            }

            //try to remove the position from the repository
            try { int result = await _repository.Positions().RemoveAsync(position); }
            catch (Exception) { throw; }    
            
            //send ok response
            return Ok("Position Id=" + id + " deleted.");
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}