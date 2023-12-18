using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models.ViewModels.VMPayroll
{
    public class VMUpdateAttendance
    {
        public List<UpdateAttendance> objList { get; set; }
        public string pEmployeeId { get; set; }
    }
}
