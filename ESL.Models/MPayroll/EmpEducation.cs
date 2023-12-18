using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class EmpEducation
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vEmployeeId { get; set; }
        [Required]
        public string vEmployeeName { get; set; }
        public string vExamp { get; set; }
        public string vGroup { get; set; }
        public string vInstitute { get; set; }
        public string vBoard { get; set; }
        public string vDivOrClass { get; set; }
        public string iYear { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
