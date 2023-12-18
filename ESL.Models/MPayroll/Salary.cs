using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Salary
    {
        /*
        vEmployeeId,iTotalDay,iHoliday,iLeaveDay,iPresentDay,iNetPayableDays,
        
        mGrossSalary,mNetPayableSalary,iTotalOTHour,mOtRate,mFridayRate,iHolidayPresentCount,mOtAmount,
        mOtherEarning,mGrossPayable,mIncomeTax,mOtherDeduction,mRevenueStamp,mAdvanceSalary,mTotalDeduction,mPayableSalary
         
        */

        public string vEmployeeId { get; set; }
        public string iTotalDay { get; set; }
        public string iHoliday { get; set; }
        public string iLeaveDay { get; set; }
        public string iPresentDay { get; set; }
        public string iNetPayableDays { get; set; }
        public decimal mGrossSalary { get; set; }
        public decimal mNetPayableSalary { get; set; }
        public decimal iTotalOTHour { get; set; }
        public decimal mOtRate { get; set; }
        public decimal mFridayRate { get; set; }
        public decimal iHolidayPresentCount { get; set; }
        public decimal mOtAmount { get; set; }
        public decimal mOtherEarning { get; set; }
        public decimal mGrossPayable { get; set; }
        public decimal mIncomeTax { get; set; }
        public decimal mOtherDeduction { get; set; }
        public decimal mRevenueStamp { get; set; }
        public decimal mAdvanceSalary { get; set; }
        public decimal mTotalDeduction { get; set; }
        public decimal mPayableSalary { get; set; }

        public string vCompanyId { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
