using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class UserDTO
    {
        /// <summary></summary>
        [Key]
        public int Id { get; set; }

        /// <summary></summary>
        [Required]
        public string Email { get; set; }

        /// <summary></summary>
        [Required]
        public string Username { get; set; }

        /// <summary></summary>
        [Required]
        public string Password { get; set; }
    }
}