using ESL.DataAccess;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models.ViewModels;
using ESL.Services.Payroll;
using ESL.TreeMenu;
using ESL.Utility;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ESL.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class PayrollReportController : Controller
    {
        private readonly ILogger<PayrollReportController> _logger;
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly UserManager<IdentityUser> _userManager;
        string hostUrl;
        private ApplicationDbContext _db { get; }
        private readonly IReport _report;

        private string reportName = "";
        private string _title = "";
        private string _userName = "";
        private string _userIp = "";
        private string caption1 = "";
        private string caption2 = "";
        private string caption3 = "";
        private string caption4 = "";
        private string caption5 = "";
        private string caption6 = "";
        private string caption7 = "";
        private string caption8 = "";


        PayrollReportService reportService;
        List<string> sqls = new List<string>();
        string sqlCon;
        public PayrollReportController
        (
            ILogger<PayrollReportController> logger,
            UnitPayrollInterface unitPayroll,
            IUnitOfWork unitOfWork,
            IBackgroundTaskQueue backgroundTaskQueue,
            UserManager<IdentityUser> userManager,
            IReport report,
            ApplicationDbContext db,
            IHttpContextAccessor accessor,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _logger = logger;
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _webHostEnvironment = webHostEnvironment;
            _backgroundTaskQueue = backgroundTaskQueue;
            _userManager = userManager;
            _report = report;
            _db = db;
            reportService = new PayrollReportService(_unitPayroll,_unitOfWork,_db,_accessor);
            hostUrl = _accessor.HttpContext.Request.Scheme + "://" + _accessor.HttpContext.Request.Host;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            setUserInfo();
        }
        private void setUserInfo()
        {
            _userIp = SD.getIp();
            var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id
            == _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (objUser != null)
            {
                _userName = objUser.UserName.Substring(0, objUser.UserName.IndexOf("@"));
            }
        }
        public IActionResult Index()
        {
            Menu menu = UserAccessor.getMyActiveMenu(_userManager, _backgroundTaskQueue, User);
            ViewBag.menu = menu;
            return View();
        }

        #region Report Parameter Load Start
        public IActionResult getServiceType(string type, string dDate, string vDepartmentId, string vSectionId)
        {
            var obj = reportService.getServiceType(type, dDate, vDepartmentId, vSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getEmployee(string type, string vStatus, string dDate, string vMonth, string vDepartmentId, string vSectionId, string vSalaryType)
        {
            var obj = reportService.getEmployee(type, vStatus, dDate, vMonth, vDepartmentId, vSectionId, vSalaryType);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getMonth(string type,string vSalaryType)
        {
            var obj = reportService.getMonth(type, vSalaryType);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getDepartment(string type, string dDate, string vMonth, string vStatus, string vSalaryType)
        {
            var obj = reportService.getDepartment(type, dDate, vMonth, vStatus, vSalaryType);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getSection(string type, string dDate, string vMonth, string vDepartmentId, string vStatus, string vSalaryType)
        {
            var obj = reportService.getSection(type,dDate,vMonth,vDepartmentId, vStatus, vSalaryType);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult getBank(string type, string vMonth)
        {
            var obj = reportService.getBank(type,vMonth);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult getBankBranch(string type,string vMonth)
        {
            var obj = reportService.getBankBranch(type,vMonth);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult getCertifiedBy(string type)
        {
            var obj = reportService.getCertifiedBy(type);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult getDesignation(string type, string vCertifiedById)
        {
            var obj = reportService.getDesignation(type, vCertifiedById);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }






        #endregion Report Parameter Load End

        #region Report Preview MasterReport Start
        public IActionResult RptEmployeeProfileBioData( string vEmployeeId)
        {
            reportName = "RptEmployeeProfileBioData.frx";
            _title = "Employee Profile (Bio-Data)";
            string sql = "select CASE WHEN ISNULL(vEmployeePhoto, '') = '' THEN '' Else '" + hostUrl + "' + replace(vEmployeePhoto, '\\','/')" +
             "END AS vEmployeePhoto,vEmployeeId,vEmployeeCode," +
             "vEmployeeName,vEmployeeNameBangla,vDesignationName,vDepartmentName,vSectionName,vReligion,dDateOfBirth,vFatherName," +
             "vContactNo,vEmailAddress,vGender,vNationality,vNationalIdNo,vEmployeeType,vServiceType," +
             "dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod," +
             "vEmployeeStatus,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup," +
             "vMaritalStatus, CASE WHEN vMaritalStatus = 'Unmarried' THEN 'N/A' ELSE CONVERT(VARCHAR, dMarriageDate, 105) END AS dMarriageDate," +
             "CASE WHEN vSpouseName=''THEN 'N/A'ELSE vSpouseName END AS vSpouseName,vSpouseOccupation,iNumberOfChild," +
             "mBasic,mHouseRent," +
             "mMedicalAllowance,mGross,mIncomeTax,mProvidentFund," +
             "vMoneyTransferType,vBankName,vAccountNo,vAccountMobileNo," +
             "CASE WHEN dStatusDate='1900-01-01' THEN 'N/A'ELSE CONVERT(VARCHAR, dStatusDate, 105) END AS dStatusDate  from tbEmployeeInfo " +
             "where vEmployeeId like '" + vEmployeeId + "' ";
            _logger.LogInformation(sql);

            string sql1 = "select * from  tbEmpEducationInfo  where vEmployeeId like '" + vEmployeeId + "' ";
            _logger.LogInformation(sql1);

            string sql2 = "select * from tbEmpExperienceInfo where vEmployeeId like '" + vEmployeeId + "' ";
            _logger.LogInformation(sql2);

            sqls.Clear();
            sqls.Add(sql);
            sqls.Add(sql1);
            sqls.Add(sql2);
       
            return ShowReport(2);
        }



        public IActionResult RptEmployeeList(string vEmployeeId,string vStatus,string vDepartmentId,string vSectionId,string vWithOrWithOutGross)
        {
            reportName = "RptEmployeeList.frx";
            _title = "Employee List";
            caption1 = vStatus;

            if (vStatus == "Active") { vStatus = "1"; }
            else if (vStatus == "Inactive") { vStatus = "0"; }
            else if (vStatus == "All") { vStatus = "%"; }

            if(vWithOrWithOutGross== "Without Gross")
            {
                reportName = "RptEmployeeList.frx";
            }
            else
            {
                reportName = "RptEmployeeListWithGross.frx";
            }

            string sql = "select * from tbEmployeeInfo where vEmployeeId like '" + vEmployeeId + "' and iStatus like '" + vStatus + "' " +
                "and vDepartmentId like '"+vDepartmentId+"' and vSectionId like '"+vSectionId+"'  " +
                "order by CAST(vEmployeeId as int)";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptEditEmployeeInfo(string vEmployeeId)
        {
            reportName = "RptEditEmployeeInfo.frx";
            _title = "Edit Employee Information";
            string sql = /*"select * from tbEmployeeInfo where vEmployeeId like '" + vEmployeeId + "' order by CAST(vEmployeeId as int) ";*/
                "select status, UserIp, UserId, EntryTime, vEmployeeId, vEmployeeCode, vEmployeeName, dDateOfBirth, dJoiningDate, mBasic," +
                " mHouseRent, mMedicalAllowance, mGross, mIncomeTax, mProvidentFund, vBankName, vBankBranchName, vAccountNo, type " +
                "from(select 'Present'status,UserIp, UserId, EntryTime, vEmployeeId, vEmployeeCode, vEmployeeName, dDateOfBirth, dJoiningDate," +
                "mBasic, mHouseRent, mMedicalAllowance, mGross, mIncomeTax, mProvidentFund, vBankName, vBankBranchName, vAccountNo, '1'type" +
                " from tbEmployeeInfo where vEmployeeId like '" + vEmployeeId + "'union all select 'OLD'status, UserIp, UserId, EntryTime, " +
                "vEmployeeId, vEmployeeCode, vEmployeeName, dDateOfBirth,dJoiningDate, mBasic, mHouseRent, mMedicalAllowance, mGross, mIncomeTax," +
                " mProvidentFund, vBankName, vBankBranchName, vAccountNo,'2'type from tbUdEmployeeInfo " +
                "where vEmployeeId like'" + vEmployeeId + "')temp " +
                "order by type, EntryTime ";

                _logger.LogInformation(sql);
                sqls.Clear();
                sqls.Add(sql);
                return ShowReport(0);
        }


        public IActionResult RptCertification(string vEmployeeId,string vCertifiedById,string  inpDesignationCertifiedBy)
        {
            reportName = "RptCertification.frx";
            _title = "Employee List";
            caption7 = vCertifiedById;
            caption8 = inpDesignationCertifiedBy;
            string sql = "select * from tbEmployeeInfo where vEmployeeId='" + vEmployeeId + "' ";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        #endregion Report Preview MasterReport End

        #region Report Preview AttendanceReport Start
        public IActionResult RptDailyAttendanceStatement( string dDate, string vDepartmentId, string vSectionId, string vServiceType, string vEmployeeType )
        {
            reportName = "RptDailyAttendanceStatement.frx";
            _title = "Daily Attendance Statement";
            caption1 = vEmployeeType;
            caption2 = vServiceType;
            if (vServiceType == "%")
            {
                caption2 = "All";
            }
            if (vEmployeeType == "Present Employee")
            {
                vEmployeeType = " where iPresentCount>0 ";
            }
            else
            {
                vEmployeeType = " ";
            }

            string sql = "select * from funDailyAttendanceForAttendanceReport " +
                "(" +
                    "'" + dDate + "','" + dDate + "','" + vDepartmentId + "','" + vSectionId + "','" + vServiceType + "','%'" +
                ") " +
                vEmployeeType + " order by vDepartmentName,vSectionName,iRank,djoiningDate";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptDailyAbsentStatement( string dDate, string vDepartmentId, string vSectionId, string vServiceType)
        {
            reportName = "RptDailyAbsentStatement.frx";
            _title = "Daily Absent Statement";
            caption1 = vServiceType;
            if (vServiceType == "%")
            {
                caption1 = "All";
            }
            string sql = "select * from funDailyAbsent('"+ vDepartmentId + "','"+ vSectionId + "','"+ vServiceType + "','%','"+ dDate + "') " +
                "order by vDepartmentName,vSectionName,iRank,dJoiningDate ";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptDailyLateInEmployeeList( string dDate, string vDepartmentId, string vSectionId, string vServiceType)
        {
            reportName = "RptDailyLateInEmployeeList.frx";
            _title = "Daily LateIn Employee List";
            caption1 = vServiceType;
            if (vServiceType == "%")
            {
                caption1 = "All";
            }
            caption2 = dDate;
            string sql = "select * from funDailyEmployeeAttendance('"+ dDate + "','"+ vDepartmentId + "','"+ vSectionId + "','"+ vServiceType + "','%','%') " +
                "where iLateCount>0 order by  vDepartmentName,vSectionName,iRank,dJoiningDate";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptDailyEarlyOutEmployeeList( string dDate, string vDepartmentId, string vSectionId, string vServiceType)
        {
            reportName = "RptDailyEarlyOutEmployeeList.frx";
            _title = "Daily EarlyOut Employee List";
            caption1 = vServiceType;
            if (vServiceType == "%")
            {
                caption1 = "All";
            }
            caption2 = dDate;
            string sql = "select * from funDailyEmployeeAttendance('" + dDate + "','" + vDepartmentId + "','" + vSectionId + "','" + vServiceType + "','%','%') " +
                "where iEarlyOutCount>0 order by  vDepartmentName,vSectionName,iRank,dJoiningDate";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }


        public IActionResult RptMonthlyDutyRoster()
        {
            reportName = "RptMonthlyDutyRoster.frx";
            _title = "Monthly Duty Roster";
            //caption2 = dDate;
            string sql = "select vEmployeeCode ,vEmployeeName,vDepartmentName,vDesignationName,vSectionName,vShiftName,vShiftType,dShiftStart," +
                "dShiftEnd,iHHDuration,iMMDuration,dFromDate,dToDate from tbDutyRoster ";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }



        public IActionResult RptIndividualEmployeeAttendance( string vMonth, string vDepartmentId, string vSectionId, string vEmployeeId)
        {
            reportName = "RptIndividualEmployeeAttendance.frx";
            _title = "Individual Employee Attendance";
            //caption2 = dDate;
            string sql ="select * from funMonthlyEmployeeAttendance " +
                "(" +
                    "'" + vMonth + "', '" + vDepartmentId + "', '" + vSectionId + "', '" + vEmployeeId + "' " +
                ")";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptMonthlyAttendanceSummary( string vMonth, string vDepartmentId, string vSectionId, string vEmployeeId )

        {
            reportName = "RptMonthlyAttendanceSummary.frx";
            _title = "Monthly Attendance Summary";
            // caption2 = dDate;
            string sql =
                "select vEmployeeCode,vEmployeeName,vDepartmentName,vSectionName,vDesignation,dTxtDate,iPresentCount, " +
                "iAbsentCount, iCLeaveCount, iSkLeaveCount,iEarLeaveCount, iMLeaveCount, iSpecialCount,iLateCount, " +
                "tourCount, mDays,iholidaycount, totalHoliday,iNotCount, " +
                "sum(iPresentCount + iCLeaveCount + iEarLeaveCount + iMLeaveCount + iSpecialCount + totalHoliday) AS netPay " +
                "from funMonthlyEmployeeAttendance " +
                "(" +
                    "'" + vMonth + "', '" + vDepartmentId + "', '" + vSectionId + "', '" + vEmployeeId + "' " +
                ") where dTxtDate='" + vMonth + "' "+
               "group by vEmployeeCode,vEmployeeName,vDepartmentName,vSectionName,vDesignation,dTxtDate,iPresentCount,iAbsentCount,iCLeaveCount,iSkLeaveCount,iEarLeaveCount," +
               "iMLeaveCount, iSpecialCount, iLateCount,tourCount, mDays,iholidaycount,totalHoliday,iNotCount ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }


        #endregion Report Preview AttendanceReport End

        #region Report Preview SalaryReport Start
        //Hello Salary

        public IActionResult RptMonthlySalary( string vMonth, string vMonthName, string vDepartmentId, string vSectionId, string vEmployeeId , string vSalaryType)
        {
            // caption2 = dDate; 

            string sql = "select * from tbMonthlySalary " +
               "where dSalaryDate = '" + vMonth + "' and  vDepartmentId like '" + vDepartmentId + "' " +
               "and vSectionId like '" + vSectionId + "' and vEmployeeId like'" + vEmployeeId + "' and ";

            if (vSalaryType == "Office Staff")
            {
                reportName = "RptMonthlySalaryOfficeStaff.frx";
                _title = "Monthly Salary Of Office Staff ("+ vMonthName+")";
                sql = sql + " vServiceType in ('Management','Office Staff') order by cast(vEmployeeCode as int) ";
            }
            else if (vSalaryType == "Factory")
            {
                reportName = "RptMonthlySalaryFacotry.frx";
                _title = "Monthly Salary Of Factory Staff ("+ vMonthName+")";
                sql = sql + " vServiceType in ('Supervisor','Driver','Factory Worker') order by cast(vEmployeeCode as int) ";
            }
            else if (vSalaryType == "Security")
            {
                reportName = "RptMonthlySalarySecurity.frx";
                _title = "Monthly Salary of Security ("+ vMonthName+")";
                sql = sql + " vServiceType in ('Security') order by cast(vEmployeeCode as int) ";
            }

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult RptPaySlip(string vMonth, string vMonthName, string vDepartmentId, string vSectionId, string vEmployeeId)
        {
            reportName = "RptPaySlip.frx";
            _title = "PaySlip ("+ vMonthName+")";
            string sql = "SELECT vEmployeeID,dGenerateDate,dSalaryDate,vSalaryMonth,vSalaryYear,vEmployeeID,vEmployeeCode,vFingerId,vEmployeeName," +
                "vDesignationID,vDesignation,vDepartmentId,vDepartmentName,vSectionID,vSectionName," +
                "vEmployeeType,vServiceType,vContactNo,dJoiningDate,iOtEnable,iHolidayEnable,iTotalOTHour," +
                "iTotalOTMin,iTotalDay,iHoliday,iWorkingDay,iAbsentDay,iLeaveDay,iLeaveWithoutPay,iPresentDay," +
                "iNetPayableDays,iHolidayPresentCount,mBasic,mHouseRent,mMedicalAllowance,mGrossSalary," +
                "mNetPayableSalary,mOtRate,mFridayRate,mOtAmount,mOtherEarning,mGrossPayable,mIncomeTax," +
                "mOtherDeduction,mRevenueStamp,mAdvanceSalary,mTotalDeduction,mPayableSalary,iLateDay,pfAmount" +
                ",mLateAmount,iRank,vBankBranchId,vBankBranchName,vBankId,vBankName,vMoneyTransferType ," +
                "CASE WHEN dJoiningDate = '1900-01-01' THEN 'N/A' ELSE CONVERT(VARCHAR, dJoiningDate, 23)  END AS JoiningDate,vAccountNo,vRoutingNo " +
                "FROM tbMonthlySalary " +
               "where dSalaryDate = '" + vMonth + "' and  vDepartmentId like '" + vDepartmentId + "' " +
               "and vSectionId like '" + vSectionId + "' and vEmployeeId like'" + vEmployeeId + "' ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }





        public IActionResult RptNoteRequisition(
            string dDate, string vMonth, string vMonthName, string vBankId, string inpDesignation,  string inpBranchName, string inpAddress, string inpAccountNo
        )
        {
            reportName = "RptNoteRequisition.frx";
            _title = "Note Requisition ("+ vMonthName+")";
            caption1 = SD.getDbToBd(dDate);
            caption2 = inpDesignation;
            caption3 = vBankId;
            caption4 = inpBranchName;
            caption5 = inpAddress;
            caption6 = inpAccountNo;

            string sql = "select sum(mNote1000)mNote1000,SUM(mNote500)mNote500,SUM(mNote200)mNote200,SUM(mNote100)mNote100,SUM(mNote50)mNote50," +
                "SUM(mNote20)mNote20,SUM(mNote10)mNote10,SUM(mNote5)mNote5,SUM(mNote2)mNote2,SUM(mNote1)mNote1,SUM(NetPay) NetPay,NetPayInWord " +
                "from funNoteRequisition " +
                "(" +
                    "'" + vMonth + "', '%', '%', '%', 'Cash' " +
                ") " +
                "group by NetPayInWord ";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        #endregion Report Preview SalaryReport End

        #region common part for Report Start
        public ActionResult ShowReport(int multidbCount)
        {
            WebReport webReport = ReportGenerate(multidbCount);
            if (webReport != null)
            {
                return PartialView("_ReportView", webReport);
            }
            return PartialView("_NoContent");
        }
        public WebReport ReportGenerate(int multidbCount)
        {
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\RPPayroll\\" + reportName;
            WebReport webReport = _report.GetReport(_accessor, path, sqls);
            if (webReport != null)
            {
                try
                {
                    for (int i = 1; i <= multidbCount; i++)
                    {
                        webReport.Report.Dictionary.Connections[i].ConnectionString = webReport.Report.Dictionary.Connections[0].ConnectionString;
                    }
                    webReport.Report.SetParameterValue("title", _title);
                webReport.Report.SetParameterValue("userName", _userName);
                webReport.Report.SetParameterValue("userIp", _userIp);
                webReport.Report.SetParameterValue("caption1", caption1);
                webReport.Report.SetParameterValue("caption2", caption2);
                webReport.Report.SetParameterValue("caption3", caption3);
                webReport.Report.SetParameterValue("caption4", caption4);
                webReport.Report.SetParameterValue("caption5", caption5);
                webReport.Report.SetParameterValue("caption6", caption6);
                webReport.Report.SetParameterValue("caption7", caption7);
                webReport.Report.SetParameterValue("caption8", caption8);
                webReport.Report.Prepare();
                    return webReport;
                }
                catch (Exception e) { _logger.LogInformation(e.Message); return null; }
            }
            return null;
        }

        #endregion Common Part for Report Show
    }
}