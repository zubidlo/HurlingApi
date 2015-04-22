using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    public class TeamDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal OverAllPoints { get; set; }

        [Required]
        public decimal LastWeekPoints { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        public int LeagueId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}