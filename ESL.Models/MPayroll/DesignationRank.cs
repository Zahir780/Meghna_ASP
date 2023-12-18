using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class DesignationRank
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vRankID { get; set; }
        [Required]
        public int iRank { get; set; }
        public string vRemarks { get; set; }
        public bool iActive { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
