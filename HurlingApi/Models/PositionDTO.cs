using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class PositionDTO
    {
        [Key]    
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}