using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPic { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
