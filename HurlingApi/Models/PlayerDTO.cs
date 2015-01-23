using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class PlayerDTO
    {
        /// <summary></summary>
        [Key]
        public int Id { get; set; }

        /// <summary></summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary></summary>
        [Required]
        public string LastName { get; set; }

        /// <summary></summary>
        [Required]
        public string GaaTeam { get; set; }

        /// <summary></summary>
        [Required]
        public decimal LastWeekPoints { get; set; }

        /// <summary></summary>
        [Required]
        public decimal OverallPoints { get; set; }

        /// <summary></summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary></summary>
        [Required]
        public byte Rating { get; set; }

        /// <summary></summary>
        [Required]
        public bool Injured { get; set; }

        /// <summary></summary>
        [Required]
        public int PositionId { get; set; }
    }
}