using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Department
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vDepartmentID { get; set; }
        [Required]
        public string vDepartmentCode { get; set; }
        [Required]
        public string vDepartmentName { get; set; }
        [Required]
        public string vDepartmentNameBangla { get; set; }
        public bool iActive { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
