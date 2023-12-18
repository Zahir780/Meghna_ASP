using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESL.Models
{
    public class ExpoToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Token { get; set; }
        public int CompanyId { get; set; }

    }
}
