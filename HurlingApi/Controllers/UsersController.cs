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
    /// <summary></summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly Repositiory<User> _repository = new Repositiory<User>(new HurlingModelContext());
        private readonly UserDTOFactory _factory = new UserDTOFactory();
       
        /// <summary></summary>
        /// <returns></returns>
        [Route("", Name = "DefaultRoute")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            var users = await _repository.GetAllAsync();
            return _factory.GetCollection(users).AsQueryable<UserDTO>();
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            try
            {
                var user = await _repository.FindAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(user));
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one user with Id=" + id + " in the repository"));
            }
        }

        /// <summary></summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("username/{username}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            try
            {
                var user = await _repository.FindAsync(u => u.Username == username);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(_factory.GetDTO(user));
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one user with Username=" + username + " in the repository."));
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO.Id != id)
            {
                return BadRequest("User Id from request URI: " + id + " doesn't match user Id from request body: " + userDTO.Id + "!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user;
            
            try
            {
                user = await _repository.FindAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                var user1 = await _repository.FindAsync(u => u.Username == userDTO.Username);
                if ((user1 != null) && (user1.Id != userDTO.Id) && (user1.Username == userDTO.Username))
                {
                    return BadRequest("There is already an user with username:" + userDTO.Username + " in the repository!");
                }
            }
            catch (InvalidOperationException)
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one user with Username=" + userDTO.Username + " in the repository."));
            }
            
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Email = userDTO.Email;

		    try
			{
			    await _repository.UpdateAsync(user);
				return StatusCode(HttpStatusCode.NoContent);
			}
			catch (InvalidOperationException e)
			{
				return InternalServerError(e);
			}
        }

        /// <summary></summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _repository.FindAsync(u => u.Username == userDTO.Username);
                if (user != null)
                {
                    return BadRequest("There is already an user with username:" + userDTO.Username + " in the repository.");
                }
            }
            catch(InvalidOperationException ) 
            {
                return InternalServerError(new Exception("Repository is broken! There is more than one user with Username=" + userDTO.Username));
            }

            var newUser = _factory.GeTModel(userDTO);
            try
            {
                await _repository.InsertAsync(newUser);
                return CreatedAtRoute("DefaultRoute", new { id = userDTO.Id }, _factory.GetDTO(newUser));
            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            try
            {
                var user = await _repository.FindAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                await _repository.RemoveAsync(user);
                return Ok(_factory.GetDTO(user));
            }
            catch(InvalidOperationException e) 
            {
                return BadRequest("Deleting user with Id=" + id + " would break referential integrity of the repository. Check the relations between this user and other entities.");
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