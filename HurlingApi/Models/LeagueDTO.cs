using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    public class LeagueDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime NextFixtures { get; set; }

        [Required]
        public byte Week { get; set; }
    }
}