using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
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
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        private readonly IRepository _repository = new FantasyHurlingRepository();
        private readonly MessageDTOFactory _factory = new MessageDTOFactory();

        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<MessageDTO>> GetMessages()
        {
            IEnumerable<Message> messages = await _repository.Messages().GetAllAsync();
            IEnumerable<MessageDTO> messageDTOs = _factory.GetDTOCollection(messages);
            IQueryable<MessageDTO> oDataMessageDTOs = messageDTOs.AsQueryable<MessageDTO>();
            return oDataMessageDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessageById(int id)
        {
            Message message;

            //try to get requested message
            try { message = await _repository.Messages().FindSingleAsync(m => m.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (message == null)
            {
                return new NotFoundActionResult(Request, "Could not find message id=" + id + ".");
            }

            var messageDTO = _factory.GetDTO(message);
            //send ok response
            return Ok(messageDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostMessage([FromBody] MessageDTO messageDTO)
        {
            //if the model state is not valit send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if a user with given id exists in the repository
            bool exist = await _repository.Users().ExistAsync(u => u.Id == messageDTO.UserId);

            //if doesn't exists send bad request response
            if (!exist)
            {
                return new ConflictActionResult(Request, "Could not find message Id=" + messageDTO.UserId + ".");
            }

            //messageDTO is ok, make new message
            var message = _factory.GeTModel(messageDTO);

            //try to insert message into repository
            try { int result = await _repository.Messages().InsertAsync(message); }
            catch (Exception) { throw; }

            //InsertAsync(message) created new id, so messageDTO must reflect that
            messageDTO = _factory.GetDTO(message);

            //send created at route response
            return Created<MessageDTO>(Request.RequestUri + "/id/" + messageDTO.Id.ToString(), messageDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteMessage([FromUri] int id)
        {
            Message message;

            //try to get a message with given id
            try { message = await _repository.Messages().FindSingleAsync(m => m.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exists send not found response
            if (message == null)
            {
                return new NotFoundActionResult(Request, "Could not find message id=" + id + "."); 
            }

            //try to delete the message
            try { int result = await _repository.Messages().RemoveAsync(message); }
            catch (Exception) { throw; }

            //send ok response
            return Ok("Message Id=" + id + " deleted.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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