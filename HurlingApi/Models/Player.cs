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
    
    public partial class Player
    {
        public Player()
        {
            this.TeamPlayers = new HashSet<TeamPlayer>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GaaTeam { get; set; }
        public decimal LastWeekPoints { get; set; }
        public decimal OverallPoints { get; set; }
        public decimal Price { get; set; }
        public byte Rating { get; set; }
        public bool Injured { get; set; }
        public int PositionId { get; set; }
    
        public virtual Position Position { get; set; }
        public virtual ICollection<TeamPlayer> TeamPlayers { get; set; }
    }
}
