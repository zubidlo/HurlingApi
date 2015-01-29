using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    interface IRepository : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<User> Users();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<League> Leagues();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<Player> Players();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<Position> Positions();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<Team> Teams();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEntity<Message> Messages();
       
    }
}
