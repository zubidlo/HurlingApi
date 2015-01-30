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
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IRepository _repository = new FantasyHurlingRepository();
        private readonly UserDTOFactory _factory = new UserDTOFactory();

        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable<UserDTO>> GetUsers()
        {
            IEnumerable<User> users = await _repository.Users().GetAllAsync();
            IEnumerable<UserDTO> userDTOs = _factory.GetDTOCollection(users);
            IQueryable<UserDTO> oDataUserDTOs = userDTOs.AsQueryable<UserDTO>();
            return oDataUserDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            User user;
            //try to get requested user
            try { user = await _repository.Users().FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null)
            {
                return new NotFoundActionResult(Request, "Could not find user Id=" + id + ".");
            }

            UserDTO userDTO = _factory.GetDTO(user);
            //send ok response
            return Ok(userDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("username/{username}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string username)
        {
            User user;
            //try to get requested user
            try { user = await _repository.Users().FindSingleAsync(u => u.Username == username); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null)
            {
                return new NotFoundActionResult(Request, "Could not find league Username=" + username + ".");
            }

            UserDTO userDTO = _factory.GetDTO(user);
            //send ok response
            return Ok(userDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpPut]
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
            try { user = await _repository.Users().FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null)
            {
                return new NotFoundActionResult(Request, "Could not find user Id=" + id + ".");
            }

            // try to get user with same username
            try { user1 = await _repository.Users().FindSingleAsync(u => u.Username == userDTO.Username); }
            catch (InvalidOperationException) { throw; }   

            //if exists and if it is different that one we are editing send bad request response
            if (user1 != null && user1.Id != id)
            {
                return new ConflictActionResult(Request, "There is already an user with name:" + userDTO.Username + " in the " +
                                    "repository! We allow only unique usernames.");
            }

            //userDTO seems ok, update the user's properties
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Email = userDTO.Email;

            //try to update the user in the repository
            try { int result = await _repository.Users().UpdateAsync(user); }
            catch (Exception) { throw; }

            //send no content response
            return Ok("User Id=" + id + " was successfully updated.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostUser([FromBody] UserDTO userDTO)
        {
            //if model state is not valid send bad request response
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //find out if there is an user with the same username
            bool exist = await _repository.Users().ExistAsync(u => u.Username == userDTO.Username);

            //if exists send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "There is already an user with name:" + userDTO.Username + " in " +
                                                "the repository. We allow only unique usernames.");
            }

            User user = _factory.GeTModel(userDTO);

            //try to insert the user into the repository
            try { int result = await _repository.Users().InsertAsync(user); }
            catch(Exception ) { throw; }
                
            //InsertAsync(user) created new id, so userDTO must reflect that
            userDTO = _factory.GetDTO(user);
                
            //send created at route response
            return Created<UserDTO>(Request.RequestUri + "/id/" + userDTO.Id.ToString(), userDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("id/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            User user;

            //try to get requested user
            try { user = await _repository.Users().FindSingleAsync(u => u.Id == id); }
            catch (InvalidOperationException) { throw; }

            //if doesn't exist send not found response
            if (user == null)
            {
                return new NotFoundActionResult(Request, "Could not find user id=" + id + "."); 
            }

            //find out if a the team referencing this user exist
            bool exist = await _repository.Teams().ExistAsync(t => t.UserId == id);

            //if exists send bad request response
            if (exist)
            {
                return new ConflictActionResult(Request, "Can't delete this user, because some team still referencing the user!");
            }

            UserDTO userDTO = _factory.GetDTO(user);

            //try to remove the user
            try { int result = await _repository.Users().RemoveAsync(user); }
            catch(Exception) { throw; }

            //send ok response
            return Ok(userDTO);
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