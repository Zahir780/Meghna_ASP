
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models
{
   public class RequestDTO
    {
        public string NotifyMe { get; set; }

        public int UserId { get; set; }
        public string ExpoToken { get; set; }
        public int CompanyId { get; set; }

    }
}
