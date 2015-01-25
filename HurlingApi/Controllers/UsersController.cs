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
       
        [Route("", Name = "DefaultRoute")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            //find all users
            var users = await _usersRepository.GetAllAsync();
            return _factory.GetCollection(users).AsQueryable<UserDTO>();
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            try
            {
                //find user with given id
                var user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(user));
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
                //find user with given username
                var user = await _usersRepository.FindAsync(u => u.Username == username);

                //check if exists
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(_factory.GetDTO(user));
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
            //check if id from URI is the same as id from request body
            if (userDTO.Id != id)
            {
                return BadRequest("User Id from request URI: " + id + " doesn't match team Id from request body: " + userDTO.Id + "!");
            }

            //check if model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //find user with given id
                var user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return BadRequest("User with id=" + id + "doesn't exist in the repository");
                }

                //find some other user with the same username
                var user1 = await _usersRepository.FindAsync(u => u.Username == userDTO.Username);

                //check if user id is different from edited user
                if (user1 != null && user1.Id != userDTO.Id)
                {
                    return BadRequest("There is already an user with name:" + userDTO.Username + " in the repository!");
                }

                //userDTO is ok, let's update the user
                user.Username = userDTO.Username;
                user.Password = userDTO.Password;
                user.Email = userDTO.Email;

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
                //find a user with same username
                var user = await _usersRepository.FindAsync(u => u.Username == userDTO.Username);

                //check if exists
                if (user != null)
                {
                    return BadRequest("There is already an team with name:" + userDTO.Username + " in the repository.");
                }

                //userDTO is ok, let's insert user
                user = _factory.GeTModel(userDTO);
                await _usersRepository.InsertAsync(user);
                return CreatedAtRoute("DefaultRoute", new { id = userDTO.Id }, _factory.GetDTO(user));
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
                //find user with given id
                var user = await _usersRepository.FindAsync(u => u.Id == id);

                //check if exists
                if (user == null)
                {
                    return BadRequest("User with id=" + id + "doesn't exist in the repository");
                }

                try
                {
                    //find a team referencing this user
                    var team = await _teamsRepository.FindAsync(t => t.UserId == id);

                    //check if found
                    if (team != null)
                    {
                        return BadRequest("Can't delete this user, because there is still one team which still have a reference to it.");
                    }
                }
                catch (InvalidOperationException)
                {
                    return BadRequest("Can't delete this user, because there are still some teams which still have a reference to it.");
                }

                //everything ok, let's remove user
                await _usersRepository.RemoveAsync(user);
                return Ok(_factory.GetDTO(user));
            }
            catch(InvalidOperationException) 
            {
                //internal server errror
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _usersRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}