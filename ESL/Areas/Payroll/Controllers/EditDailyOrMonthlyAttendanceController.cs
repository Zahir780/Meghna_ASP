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
    public class EditDailyOrMonthlyAttendanceController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<EditDailyOrMonthlyAttendanceController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private EditDailyOrMonthlyAttendanceService service;

        public EditDailyOrMonthlyAttendanceController
        (
            UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<EditDailyOrMonthlyAttendanceController> logger,
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
            service = new EditDailyOrMonthlyAttendanceService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAttendance (string vAttendanceBy, string dDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            var obj = service.GetAttendance(vAttendanceBy, dDate, pEmployeeId, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }

        public IActionResult updateAttendance(VMUpdateAttendance objVM)
        {
            if (isValid(objVM.objList, objVM.pEmployeeId))
            {
                int a = service.updateAttendance(objVM.objList);
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successEditMessage });
                }
            }
            return Json(new { success = false, message = SD.errorMessage });
        }
        public IActionResult getMonth()
        {
            var obj = service.getMonth();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getDepartment(string vAttendanceBy, string dDate)
        {
            var obj = service.getDepartment(vAttendanceBy, dDate);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getSection(string vAttendanceBy, string dDate, string pDepartmentId)
        {
            var obj = service.getSection(vAttendanceBy, dDate, pDepartmentId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getEmployee(string vAttendanceBy, string dDate, string pDepartmentId, string pSectionId)
        {
            var obj = service.getEmployee(vAttendanceBy, dDate, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        private bool isValid( List<UpdateAttendance> objList, string pEmployeeId)
        {
            if (!SD.isEmpty(objList[0].vEmployeeId) && !SD.isEmpty(pEmployeeId))
            {
                return true;
            }
            return false;
        }
    }
}
