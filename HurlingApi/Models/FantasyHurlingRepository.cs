using System;
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

namespace HurlingApi.Models
{
    public class FantasyHurlingRepository : IRepository
    {
        private readonly DbContext _context;
        private readonly IEntity<User> userTable; 
        private readonly IEntity<League> leagueTable;
        private readonly IEntity<Player> playerTable;
        private readonly IEntity<Position> positionTable;
        private readonly IEntity<Team> teamTable;
        private readonly IEntity<Message> messageTable;

        bool _disposed;

        public FantasyHurlingRepository()
        {
            _context = new HurlingModelContext();
            userTable = new Entity<User>(_context);
            leagueTable = new Entity<League>(_context);
            playerTable = new Entity<Player>(_context);
            positionTable = new Entity<Position>(_context);
            teamTable = new Entity<Team>(_context);
            messageTable = new Entity<Message>(_context);
        }

        public IEntity<User> Users()
        {
            return userTable;
        }

        public IEntity<League> Leagues()
        {
            return leagueTable;
        }

        public IEntity<Player> Players()
        {
            return playerTable;
        }

        public IEntity<Position> Positions()
        {
            return positionTable;
        }

        public IEntity<Team> Teams()
        {
            return teamTable;
        }

        public IEntity<Message> Messages()
        {
            return messageTable;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context.Dispose();
                userTable.Dispose();
                leagueTable.Dispose();
                playerTable.Dispose();
                positionTable.Dispose();
                teamTable.Dispose();
                messageTable.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        ~FantasyHurlingRepository()
        {
            Dispose(false);
        }
    }
}