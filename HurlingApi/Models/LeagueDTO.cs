using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class LeagueDTO
    {
        /// <summary></summary>
        [Key]
        public int Id { get; set; }

        /// <summary></summary>
        [Required]
        public string Name { get; set; }

        /// <summary></summary>
        [Required]
        public DateTime NextFixtures { get; set; }

        /// <summary></summary>
        [Required]
        public byte Week { get; set; }
    }
}