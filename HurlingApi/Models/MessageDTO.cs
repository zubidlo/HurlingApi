using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HurlingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public Nullable<System.DateTime> Created { get; set; }
    }
}