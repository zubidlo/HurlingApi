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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly Repositiory<User> _usersRepository = new Repositiory<User>(new HurlingModelContext());
        private readonly Repositiory<Team> _teamsRepository = new Repositiory<Team>(new HurlingModelContext());
        private readonly UserDTOFactory _factory = new UserDTOFactory();
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _usersRepository.Dispose();
                    _teamsRepository.Dispose();
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }
       
        [Route("", Name = "DefaultRoute")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            IEnumerable<User> users = await _usersRepository.GetAllAsync();
            IEnumerable<UserDTO> userDTOs = _factory.GetDTOCollection(users);
            return userDTOs.AsQueryable<UserDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            try
            {
                //get requested user
                User user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                UserDTO userDTO = _factory.GetDTO(user);
                return Ok(userDTO);
            }
            catch (InvalidOperationException)
            {
                //internal server error
                throw;
            }
        }

        [Route("name/{name}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            try
            {
                //get requested user
                User user = await _usersRepository.FindAsync(u => u.Username == username);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                UserDTO userDTO = _factory.GetDTO(user);
                return Ok(userDTO);
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
        public async Task<IHttpActionResult> EditUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            //check if id from URI matches Id from request body
            if (id != userDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match the Id from request body: " + userDTO.Id + "!");
            }

            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get requested user
                User user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                //get user with same username
                var user1 = await _usersRepository.FindAsync(u => u.Username == userDTO.Username);

                //check if exists and if it is different that one we are editing
                if (user1 != null && user1.Id != id)
                {
                    return BadRequest("There is already an user with name:" + userDTO.Username + " in the repository! We allow only unique usernames.");
                }

                //userDTO seems ok, update the user
                user.Username = userDTO.Username;
                user.Password = userDTO.Password;
                user.Email = userDTO.Email;

                //user must be the reference to actual user in the repository. UpdateAsync will throw exception otherwise.
                //I can't just UpdateAsync(new User());
                await _usersRepository.UpdateAsync(user);
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
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //get a user with same username
                User user = await _usersRepository.FindAsync(u => u.Username == userDTO.Username);

                //check if exists
                if (user != null)
                {
                    return BadRequest("There is already an user with name:" + userDTO.Username + " in the repository. We allow only unique usernames.");
                }

                //userDTO is ok, insert new user
                user = _factory.GeTModel(userDTO);
                await _usersRepository.InsertAsync(user);
                
                //InsertAsync(user) created new id, so userDTO must reflect that
                userDTO = _factory.GetDTO(user);
                
                return CreatedAtRoute("DefaultRoute", new { id = user.Id }, userDTO);
            }
            catch(InvalidOperationException ) 
            {
                //internal server error
                throw;
            }
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            try
            {
                //get requested user
                User user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                try
                {
                    //find a team referencing this user
                    Team team = await _teamsRepository.FindAsync(t => t.UserId == id);

                    //check if exists
                    if (team != null)
                    {
                        return BadRequest("Can't delete this user, because team id=" + team.Id + " still referencing the user!");
                    }
                }
                catch (InvalidOperationException)
                {
                    return BadRequest("Can't delete this user, because there are still some teams referencing the user!");
                }

                //everything is ok, remove the user
                await _usersRepository.RemoveAsync(user);
                UserDTO userDTO = _factory.GetDTO(user);
                return Ok(userDTO);
            }
            catch(InvalidOperationException) 
            {
                //internal server errror
                throw;
            }
        }
    }
}