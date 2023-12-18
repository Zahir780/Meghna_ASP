using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public bool IsAuthorizedCompany { get; set; }
        public DateTime? Persistance { get; set; }
        public int Status { get; set; }
        public string? Email { get; set; }
    }
}
