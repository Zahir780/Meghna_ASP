using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models.ViewModels.VMPayroll
{
    public class PayrollReportVM
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        /*public string OPStockOrderBy { get; set; }
        public string IssueRegisterOrderBy { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
        public IEnumerable<SelectListItem> ItemList { get; set; }
        public IEnumerable<SelectListItem> SupplierList { get; set; }
        public IEnumerable<SelectListItem> WareHouseList { get; set; }*/
    }
}
