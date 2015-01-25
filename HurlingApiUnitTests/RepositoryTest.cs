using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Models;
using System.Threading.Tasks;

namespace HurlingApiUnitTests
{
    [TestClass]
    public class UserRepositoryTest : IDisposable
    {
        private IRepository<User> _repository;
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserRepositoryTest()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _repository.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        [TestInitialize]
        public void Initialize()
        {
            _repository = new Repositiory<User>(new HurlingModelContext());
        }

        [TestMethod]
        public async Task QueryAllUsers()
        {
            var allUsers = await _repository.GetAllAsync();
            Assert.AreEqual(allUsers.Count(), 4);
        }

        [TestMethod]
        public async Task GetUserById()
        {
            int id = 1;
            var user = await _repository.FindAsync(u => u.Id == id);
            Assert.AreEqual(user.Id, id);
        }

        [TestMethod]
        public async Task GetUserByUsername()
        {
            string username = "zubidlo";
            var user = await _repository.FindAsync(u => u.Username == username);
            Assert.AreEqual(user.Username, username);
        }

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
