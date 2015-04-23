using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Controllers;
using HurlingApi.Models;
using System.Collections.Generic;

namespace HurlingApiTests
{
    class TestRepository : IRepository
    {
        public IEntity<Position> Positions()
        {
            return new PositionTestEntity();
        }

        public IEntity<User> Users()
        {
            throw new NotImplementedException();
        }

        public IEntity<League> Leagues()
        {
            throw new NotImplementedException();
        }

        public IEntity<Player> Players()
        {
            throw new NotImplementedException();
        }

        public IEntity<Team> Teams()
        {
            throw new NotImplementedException();
        }

        public IEntity<Message> Messages()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
