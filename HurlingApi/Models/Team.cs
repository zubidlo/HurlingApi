//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HurlingApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal LeaguePoints { get; set; }
        public Nullable<decimal> LastWeekPoints { get; set; }
        public Nullable<decimal> Budget { get; set; }
    
        public virtual ICollection<Player> Players { get; set; }
        public virtual League League { get; set; }
        public virtual User User { get; set; }
    }
}
