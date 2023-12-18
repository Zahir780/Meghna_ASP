using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECL.Services.Payroll;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels.VMPayroll;
using ESL.Services;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESL.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class DateBetweenAttendanceManuallyMachineController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<DateBetweenAttendanceManuallyMachineController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private DateBetweenAttendanceManuallyMachineService service;

        public DateBetweenAttendanceManuallyMachineController
        (
            UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<DateBetweenAttendanceManuallyMachineController> logger,
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            IWebHostEnvironment hostEnvironment
        )
        {
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _logger = logger;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _hostEnvironment = hostEnvironment;
            service = new DateBetweenAttendanceManuallyMachineService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAttendance ( string pEmployeeId, string pDepartmentId, string pSectionId )
        {
            var obj = service.GetAttendance(pEmployeeId, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }

        public IActionResult generateAttendance(VMAttendance objVM)
        {
            if (isValid(objVM.objList, objVM.dFromDate, objVM.dToDate, objVM.inTime, objVM.outtime))
            {
                int a = service.generateAttendance(
                    objVM.objList, objVM.dFromDate, objVM.dToDate, objVM.inTime, objVM.outtime
                );
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successEditMessage });
                }
            }
            return Json(new { success = false, message = SD.errorMessage });
        }
        public IActionResult getDepartment()
        {
            var obj = service.getDepartment();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getSection(string pDepartmentId)
        {
            var obj = service.getSection(pDepartmentId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getEmployee(string pDepartmentId, string pSectionId)
        {
            var obj = service.getEmployee(pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        private bool isValid (
            string pEmployeeId, string pDepartmentId, string pSectionId 
        )
        {
            if (pEmployeeId != "")
            {
                return true;
            }
            return false;
        }
        private bool isValid(
                List<Attendance> objList, string dFromDate, string dToDate, string inTime, string outtime
            )
        {
            if (!SD.isEmpty(objList[0].vEmployeeId) && !SD.isEmpty(dFromDate) && !SD.isEmpty(dToDate))
            {
                return true;
            }
            return false;
        }
    }
}
