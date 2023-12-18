using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Holiday
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }        
        [Required]
        public DateTime dDate { get; set; }
        [Required]
        public string vOccasion { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
