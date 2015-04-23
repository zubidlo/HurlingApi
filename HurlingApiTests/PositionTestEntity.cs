using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HurlingApi.Controllers;
using HurlingApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HurlingApiTests
{
    class PositionTestEntity : IEntity<Position>
    {
        private readonly List<Position> positions;

        public PositionTestEntity()
        {
            positions = new List<Position>();
            positions.Add(new Position { Id = 1, Name = "position1" });
            positions.Add(new Position { Id = 2, Name = "position2" });
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await Task.Run(() => positions);
        }

        public System.Threading.Tasks.Task<Position> FindSingleAsync(System.Linq.Expressions.Expression<Func<Position, bool>> match)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<bool> ExistAsync(System.Linq.Expressions.Expression<Func<Position, bool>> match)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> UpdateAsync(Position t)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> InsertAsync(Position t)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> RemoveAsync(Position t)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
