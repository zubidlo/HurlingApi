using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Controllers;
using HurlingApi.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;

namespace HurlingApiTests
{
    [TestClass]
    public class PositionControllerTests
    {
        //Arrange
        private static readonly IRepository _repository = new TestRepository();
        private static readonly PositionsController _controller = new PositionsController(_repository);
        
        [TestMethod]
        public async Task GetPositions_ShouldReturnAllPositions()
        {
            //Act
            var expected = await _repository.Positions().GetAllAsync();
            var returned = await _controller.GetPostions();

            //Assert
            Assert.AreEqual(expected.Count(), returned.Count());
        }
    }
}
