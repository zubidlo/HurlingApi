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
        private HurlingModelContext db = new HurlingModelContext();
        private DTOFactory factory = new DTOFactory();

        // GET: api/users
        /// <summary>
        /// Returns all users. This method supports OData filters.
        /// </summary>
        [Route("")]
        public IQueryable<UserDTO> GetUsers()
        {
            var users = db.Users;
            var userDTOs = factory.GetAllUserDTOs(users).AsQueryable<UserDTO>();
            return userDTOs;
        }

        // GET : api/users/username
        /// <summary>
        /// Returns user with given username or 404/Not Found
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        [Route("{Username}")]
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserByUsername([FromUri] string Username)
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Username == Username);
           
            if (user == null)
            {
                return NotFound();
            }
            
            var userDTO = factory.GetUserDTO(user);
            return Ok(userDTO);
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserDTO))]
        public async Task<IHttpActionResult> GetUserById([FromUri] int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = factory.GetUserDTO(user);
            return Ok(userDTO);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser([FromUri] int id, [FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest("user with id:" + id + " doesn't exist" );
            }
            
            user = factory.GetUser(userDTO);

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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

            var user = factory.GetUser(userDTO);

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser([FromUri] int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}