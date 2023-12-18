using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Section
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vSectionID { get; set; }
        [Required]
        public string vSectionCode { get; set; }
        [Required]
        public string vSectionName { get; set; }
        [Required]
        public string vSectionNameBangla { get; set; }
        public bool iActive { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
