using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    public class PlayerDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string GaaTeam { get; set; }

        public Nullable<decimal> LastWeekPoints { get; set; }
        public Nullable<decimal> OverallPoints { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public byte Rating { get; set; }
        [Required]
        public bool Injured { get; set; }
        [Required]
        public int PositionId { get; set; }
    }
}