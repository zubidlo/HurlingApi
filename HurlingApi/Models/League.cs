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
    
    public partial class League
    {
        public League()
        {
            this.Teams = new HashSet<Team>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime NextFixtures { get; set; }
        public byte Week { get; set; }
    
        public ICollection<Team> Teams { get; set; }
    }
}
