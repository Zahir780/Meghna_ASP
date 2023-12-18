using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models.ViewModels.VMPayroll
{
    public class VMAttendance
    {
        public List<Attendance> objList { get; set; }

        public string dFromDate { get; set; }
        public string dToDate { get; set; }
        public string inTime { get; set; }
        public string outtime { get; set; }
    }
}
