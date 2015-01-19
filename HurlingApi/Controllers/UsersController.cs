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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly HurlingModelContext _context = new HurlingModelContext();
        private readonly Repositiory<User> _repository = new Repositiory<User>();
        private readonly UserFactoryDTO _factory = new UserFactoryDTO();
       
        /// <summary>
        /// Gets all users.
        /// This method supports OData filters, for example:
        /// /api/users?$orderby=Username : will return users sorted by username
        /// </summary>
        [Route("", Name = "DefaultRoute")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            var users = await _repository.GetAllAsync();
            var userDTOs = _factory.GetCollection(users).AsQueryable<UserDTO>();
            return userDTOs;
        }

        /// <summary>
        /// Gets user by his Id.
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

            var userDTO = _factory.GetDTO(user);
            return Ok(userDTO);
        }

        /// <summary>
        /// Gets user by his Username
        /// </summary>
        /// <param name="username">The Username of the user.</param>
        [Route("username/{username}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            User user = null;
            try
            {
                user = await _repository.FindAsync(u => u.Username == username);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("There is more than one user with username:" + username + " in the repository...");
            }
            
            if (user == null)
            {
                return NotFound();
            }
            
            var userDTO = _factory.GetDTO(user);
            return Ok(userDTO);
        }

        /// <summary>
        /// Updates all user fields except the Id.
        /// </summary>
        /// <param name="id">The Id of the user.</param>
        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO.Id != id)
            {
                return BadRequest("User Id from URI: " + id + " doesn't match JSON user Id in request body: " + userDTO.Id + "!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.FindAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("User with Id:" + id + " doesn't exist in repository!" );
            }

            User user2 = null;

            try
            {
                user2 = await _repository.FindAsync(u => u.Username == userDTO.Username);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("There is already more then one user with username:" + userDTO.Username + " in the repository...");
            }

            if ((user2 != null ) && (user2.Id != userDTO.Id) && (user2.Username == userDTO.Username))
            {
                return BadRequest("There is already an user with username:" + userDTO.Username + " in the repository...");
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

        /// <summary>
        /// Inserts a new user. Id value will be ignored.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = null;

            try
            {
                user = await _repository.FindAsync(u => u.Username == userDTO.Username);
            }
            catch(InvalidOperationException e) 
            {
                return BadRequest("There is more than one model with username:" + userDTO.Username + " in the repository...");
            }

            if (user != null)
            {
                return BadRequest("There is already an model with username:" + userDTO.Username + " in the repository...");
            }

            user = _factory.GeTModel(userDTO);
            await _repository.InsertAsync(user);
            userDTO = _factory.GetDTO(user);

            return CreatedAtRoute("DefaultRoute", new { id = userDTO.Id }, userDTO);
        }

        // DELETE: api/Users/id/5 (not implemented just yet)
        /// <summary>
        /// Deletes the user.
        /// </summary>
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
            return _context.Users.Count(u => u.Id == id) > 0;
        }
    }
}