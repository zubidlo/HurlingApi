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
        [Route("", Name = "messagesRoute")]
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
        [ResponseType(typeof(MessageDTO))]
        public async Task<IHttpActionResult> GetMessageById(int id)
        {
            Message message;

            //try to get requested message
            try { message = await _repository.Messages().FindSingleAsync(m => m.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (message == null) { return NotFound(); }

            MessageDTO messageDTO = _factory.GetDTO(message);
            //send ok response
            return Ok(messageDTO);
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