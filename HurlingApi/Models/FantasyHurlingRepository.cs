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
    /// <summary>
    /// 
    /// </summary>
    public class FantasyHurlingRepository : IDisposable
    {
        private readonly DbContext _context = new HurlingModelContext();
        private readonly IEntity<User> userTable; 
        private readonly IEntity<League> leagueTable;
        private readonly IEntity<Player> playerTable;
        private readonly IEntity<Position> positionTable;
        private readonly IEntity<Team> teamTable;

        bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        public FantasyHurlingRepository()
        {
            userTable = new Entity<User>(_context);
            leagueTable = new Entity<League>(_context);
            playerTable = new Entity<Player>(_context);
            positionTable = new Entity<Position>(_context);
            teamTable = new Entity<Team>(_context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity<User> Users()
        {
            return userTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity<League> Leagues()
        {
            return leagueTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity<Player> Players()
        {
            return playerTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity<Position> Positions()
        {
            return positionTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEntity<Team> Teams()
        {
            return teamTable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ~FantasyHurlingRepository()
        {
            Dispose(false);
        }
    }
}