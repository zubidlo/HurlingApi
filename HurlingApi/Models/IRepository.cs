using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    public interface IRepository : IDisposable
    {
        IEntity<User> Users();
        IEntity<League> Leagues();
        IEntity<Player> Players();
        IEntity<Position> Positions();
        IEntity<Team> Teams();
        IEntity<Message> Messages();
    }
}
