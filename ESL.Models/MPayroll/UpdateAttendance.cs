using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class UpdateAttendance
    {
        public string vEmployeeId { get; set; }
        public string dAttendanceDate { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vPermittedBy { get; set; }
        public string vReason { get; set; }
        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
