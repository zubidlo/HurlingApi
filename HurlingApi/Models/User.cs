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

    /// <summary></summary>
    public partial class User
    {
        /// <summary></summary>
        public User()
        {
            this.Messages = new HashSet<Message>();
        }

        /// <summary></summary>
        public int Id { get; set; }

        /// <summary></summary>
        public string Email { get; set; }

        /// <summary></summary>
        public string Username { get; set; }

        /// <summary></summary>
        public string Password { get; set; }

        /// <summary></summary>
        public virtual ICollection<Message> Messages { get; set; }

        /// <summary></summary>
        public virtual Team Team { get; set; }
    }
}
