using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Models;

namespace HurlingApiUnitTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        private IRepository<User> _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new Repositiory<User>();
        }

        [TestMethod]
        public async void QueryAllUsers()
        {
            var allUsers = await _repository.GetAllAsync();
            Assert.AreEqual(allUsers.Count(), 3);
        }
    }
}
