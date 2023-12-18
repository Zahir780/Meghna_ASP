using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESL.Models
{
   public class UserAuthentication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AccessUserId { get; set; }
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int MenuId { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        [Required]
        public Boolean IsAdd { get; set; }
        [Required]
        public Boolean IsEdit { get; set; }
        [Required]
        public Boolean IsDelete { get; set; }
       // [Required]
        public Boolean IsPreview { get; set; }
        [Required]
        public Boolean IsReport { get; set; }
        [Required]
        public Boolean IsExcel { get; set; }
        [Required]
        public Boolean IsWord { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
    }
}
