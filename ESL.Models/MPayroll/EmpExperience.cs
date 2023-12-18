using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class EmpExperience
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vEmployeeId { get; set; }
        [Required]
        public string vEmployeeName { get; set; }
        public string vPostOrDesignation { get; set; }
        public string vCompanyName { get; set; }
        public string dFrom { get; set; }
        public string dTo { get; set; }
        public string vMajorResponsibility { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
