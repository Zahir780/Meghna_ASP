using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models
{
   public class ResponseDTO
    {
        public string Connection { get; set; }
        public int status { get; set; }
        public Boolean isFound { get; set; }
        public String message { get; set; }

        public Boolean isAuthorizeCompany { get; set; }

        public string cashHead { get; set; }
        public string cashHeadId { get; set; }
    }
}
