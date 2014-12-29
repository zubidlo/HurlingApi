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

namespace HurlingApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly HurlingModelContext _context = new HurlingModelContext();
        private readonly Repositiory<User> _repository = new Repositiory<User>();
        private readonly DTOFactory _factory = new DTOFactory();
       
        // GET: api/users
        /// <summary>
        /// Returns json array of users in response body.
        /// Can return empty array.
        /// This method supports OData filters:
        /// /api/users?$orderby=Username : will return users sorted by username
        /// </summary>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            var users = await _repository.GetAllAsync();
            var userDTOs = _factory.UserDTOs(users).AsQueryable<UserDTO>();
            return userDTOs;
        }

        // GET: api/users/id{id}
        /// <summary>
        /// Returns success: 200/OK with json user in response body
        /// Returns error: 404/Not Found
        /// </summary>
        /// <param name="id">The Id of the user.</param>
        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            var user = await _repository.FindAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = _factory.UserDTO(user);
            return Ok(userDTO);
        }

        // GET : api/users/username{username}
        /// <summary>
        /// Returns success: 200/Ok with json user in response body 
        /// Returns error: 404/Not Found
        /// </summary>
        /// <param name="username">The Username of the user.</param>
        [Route("username/{username}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            var user = await _repository.FindAsync(u => u.Username == username);
           
            if (user == null)
            {
                return NotFound();
            }
            
            var userDTO = _factory.UserDTO(user);
            return Ok(userDTO);
        }

        // PUT: api/users/id/{id}
        /// <summary>
        /// Updates all user fields except Id. id from uri must match json user id in request body and existing user Id.
        /// Returns success: 204/No Content
        /// Returns error: 400/BadRequest with short error message
        /// </summary>
        /// <param name="id">The Id of the user.</param>
        [Route("id/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO.Id != id)
            {
                return BadRequest("user id from uri: " + id + " doesn't match json user id in request body: " + userDTO.Id);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.FindAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("user with id:" + id + " doesn't exist" );
            }

            user.Email = userDTO.Email;
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;

            try
            {
                await _repository.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _factory.User(userDTO);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Count(e => e.Id == id) > 0;
        }
    }
}