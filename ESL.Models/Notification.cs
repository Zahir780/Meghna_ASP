using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;

namespace ESL.Models
{
    public class Notification
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int EmployeeVerifyId { get; set; }

        [Required]
        public int Type { get; set; }
        [Required]
        public int IsRead { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}
