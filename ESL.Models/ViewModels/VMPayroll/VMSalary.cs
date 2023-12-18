using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models.ViewModels.VMPayroll
{
    public class VMSalary
    {
        public List<Salary> objList { get; set; }
        public string pSalaryDate { get; set; }
        public string pEmployeeId { get; set; }
        public string pDepartmentId { get; set; }
        public string pSectionId { get; set; }
    }
}
