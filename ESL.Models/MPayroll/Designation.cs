using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
    //Designation
{
    public class Designation
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vDesignationID { get; set; }
        [Required]
        public string vDesignationCode { get; set; }
        [Required]
        public int iRank { get; set; }
        [Required]
        public string vDesignationName { get; set; }
        [Required]
        public string vDesignationNameBangla { get; set; }
        public bool iActive { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
