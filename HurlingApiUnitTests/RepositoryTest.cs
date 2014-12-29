using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Models;
using System.Threading.Tasks;

namespace HurlingApiUnitTests
{
    /// <summary>
    /// Tests user repository
    /// </summary>
    [TestClass]
    public class UserRepositoryTest
    {
        private IRepository<User> _repository;

        /// <summary>
        /// initializes repository
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _repository = new Repositiory<User>();
        }

        /// <summary>
        /// test: get all users and then count them.
        /// pass: when count = 4.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task QueryAllUsers()
        {
            var allUsers = await _repository.GetAllAsync();
            Assert.AreEqual(allUsers.Count(), 4);
        }

        /// <summary>
        /// test: find user with id=1.
        /// pass: if user.Id == 1.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUserById()
        {
            int id = 1;
            var user = await _repository.FindAsync(u => u.Id == id);
            Assert.AreEqual(user.Id, id);
        }

        /// <summary>
        /// test: find user with username="zubidlo".
        /// pass: if user.Username == "zubidlo".
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUserByUsername()
        {
            string username = "zubidlo";
            var user = await _repository.FindAsync(u => u.Username == username);
            Assert.AreEqual(user.Username, username);
        }

        /// <summary>
        /// test: insert new user and then find that user back
        /// pass: if new user and user returned back are the same
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Ignore]
        public async Task InsertNewUser()
        {
            var newUser = new User()
            {
                Email = "newEmail",
                Username = "newUsername",
                Password = "newPassword"
            };

            await _repository.InsertAsync(newUser);
            var userBack = await _repository.FindAsync(u => u.Username == newUser.Username);
            Assert.AreEqual(newUser, userBack);

        }

        /// <summary>
        /// test: delete user with id=5
        /// pass: if one row in table affected result came back
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Ignore]
        public async Task DeleteAUser()
        {
            var id = 5;
            var user = await _repository.FindAsync(u => u.Id == id);
            var result = await _repository.RemoveAsync(user);
            Assert.AreEqual(result, 1);
        }
    }
}
