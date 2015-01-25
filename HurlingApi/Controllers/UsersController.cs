﻿using System;
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
        private readonly Repositiory<User> _usersRepository = 
            new Repositiory<User>(new HurlingModelContext());
        private readonly Repositiory<Team> _teamsRepository = 
            new Repositiory<Team>(new HurlingModelContext());
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
            IQueryable<UserDTO> oDataUserDTOs = userDTOs.AsQueryable<UserDTO>();
            return oDataUserDTOs;
        }

        [Route("id/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            User user;
            //try to get requested user
            try { user = await _usersRepository.FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null) { return NotFound(); }

            UserDTO userDTO = _factory.GetDTO(user);
            //send ok response
            return Ok(userDTO);
        }

        [Route("username/{username}")]
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            User user;
            //try to get requested user
            try { user = await _usersRepository.FindSingleAsync(u => u.Username == username); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null) { return NotFound(); }

            UserDTO userDTO = _factory.GetDTO(user);
            //send ok response
            return Ok(userDTO);
        }

        [Route("id/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            //if id from URI matches Id from request body send bad request response
            if (id != userDTO.Id)
            {
                return BadRequest("The id from URI: " + id + " doesn't match " +
                                "the Id from request body: " + userDTO.Id + "!");
            }

            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            User user, user1;

            //try to get requested user
            try { user = await _usersRepository.FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null) { return NotFound(); }

            // try to get user with same username
            try { user1 = await _usersRepository.FindSingleAsync(u => u.Username == userDTO.Username); }
            catch (InvalidOperationException) { throw; }   

            //if exists and if it is different that one we are editing send bad request response
            if (user1 != null && user1.Id != id)
            {
                return BadRequest("There is already an user with name:" + userDTO.Username + " in the " +
                                    "repository! We allow only unique usernames.");
            }

            //userDTO seems ok, update the user's properties
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Email = userDTO.Email;

            //try to update the user in the repository
            try { int result = await _usersRepository.UpdateAsync(user); }
            catch (Exception) { throw; }

            //send no content response
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if there is an user with the same username
            bool exist = await _usersRepository.ExistAsync(u => u.Username == userDTO.Username);

            //if exists send bad request response
            if (exist)
            {
                return BadRequest("There is already an user with name:" + userDTO.Username + " in " +
                                    "the repository. We allow only unique usernames.");
            }

            User user = _factory.GeTModel(userDTO);

            //try to insert the user into the repository
            try { int result = await _usersRepository.InsertAsync(user); }
            catch(Exception ) { throw; }
                
            //InsertAsync(user) created new id, so userDTO must reflect that
            userDTO = _factory.GetDTO(user);
                
            //send created at route response
            return CreatedAtRoute("DefaultRoute", new { id = user.Id }, userDTO);
        }

        [Route("id/{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            User user;

            //try to get requested user
            try { user = await _usersRepository.FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null) { return NotFound(); }

            //find out if a the team referencing this user exist
            bool exist = await _teamsRepository.ExistAsync(t => t.UserId == id);

            //if exists send bad request response
            if (exist)
            {
                return BadRequest("Can't delete this user, because some team still referencing the user!");
            }

            //try to remove the user
            try { int result = await _usersRepository.RemoveAsync(user); }
            catch(Exception) { throw; }

            UserDTO userDTO = _factory.GetDTO(user);

            //send ok response
            return Ok(userDTO);
        }
    }
}