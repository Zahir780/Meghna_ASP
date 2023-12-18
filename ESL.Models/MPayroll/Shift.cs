using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Shift
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }        
        [Required]
        public int vShiftId { get; set; }
        [Required]
        public string vShiftName { get; set; }
        [Required]
        public string vShiftType { get; set; }
        [Required]
        public TimeSpan dShiftStart { get; set; }
        [Required]
        public TimeSpan dShiftEnd { get; set; }
        [Required]
        public TimeSpan dLateInLimit { get; set; }
        [Required]
        public TimeSpan dEarlyOutLimit { get; set; }
        [Required]
        public int iHHDuration { get; set; }
        [Required]
        public int iMMDuration { get; set; }
        public bool iActive { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
