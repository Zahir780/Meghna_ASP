using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Bank
    {
        [Key]
        [Required]
        public int Id { get; set; }       
        public string BankName { get; set; }
        public string BankLogo { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
