using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HurlingApi.Models;

namespace HurlingApiUnitTests
{
    [TestClass]
    public class FactoryTest
    {
        private UserFactoryDTO _userFactory;

        [TestInitialize]
        public void Initialize()
        {
            _userFactory = new UserFactoryDTO();
        }

        [TestMethod]
        public void TestUserFactoryGetDTO()
        {
            var user = new User()
            {
                Id = 1,
                Email = "dfdfd",
                Username = "dfdfd",
                Password = "sdfd"
            };

            var userDTO = _userFactory.GetDTO(user);
            Assert.AreEqual(userDTO.Id, user.Id);

        }

        [TestMethod]
        public void TestUserFactoryGetModel()
        {
            var userDTO = new UserDTO()
            {
                Id = 1,
                Email = "dfdfd",
                Username = "dfdfd",
                Password = "sdfd"
            };

            var user = _userFactory.GeTModel(userDTO);
            Assert.AreEqual(user.Id, userDTO.Id);
        }

        [TestMethod]
        public void TestUserFactoryGetCollection()
        {
            var set = new HashSet<User>();
            var user = new User()
            {
                Id = 1,
                Email = "a",
                Username = "b",
                Password = "c"
            };

            set.Add(user);

            var setDTOs = _userFactory.GetCollection(set);

            Assert.IsNotNull(setDTOs);
        }
    }
}
