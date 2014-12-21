using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class PositionDTO
    {
        public PositionDTO()
        {
            this.PlayerIds = new HashSet<int>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> PlayerIds { get; set; }
    }
}