using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class TeamDTO
    {
        /// <summary></summary>
        [Key]
        public int Id { get; set; }

        /// <summary></summary>
        [Required]
        public string Name { get; set; }

        /// <summary></summary>
        [Required]
        public decimal OverAllPoints { get; set; }

        /// <summary></summary>
        [Required]
        public decimal LastWeekPoints { get; set; }

        /// <summary></summary>
        [Required]
        public decimal Budget { get; set; }

        /// <summary></summary>
        [Required]
        public int LeagueId { get; set; }

        /// <summary></summary>
        [Required]
        public int UserId { get; set; }
    }
}