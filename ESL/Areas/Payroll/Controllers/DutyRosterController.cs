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
    public class DutyRosterController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<DutyRosterController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private DutyRosterService service;

        public DutyRosterController(UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<DutyRosterController> logger,
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
            service = new DutyRosterService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult getShiftData()
        {
            var obj = service.getShiftData();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult Generate(string pDate, string pFromDate, string pToDate, string pEmployeeId, string pDepartmentId, string pSectionId, string pShiftId)
        {
            if (isValid(pDate, pFromDate, pToDate, pEmployeeId, pDepartmentId, pSectionId, pShiftId))
            {
                int a = service.Generate(pDate, pFromDate, pToDate, pEmployeeId, pDepartmentId, pSectionId, pShiftId);
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successSaveMessage });
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
        public IActionResult checkDutyRoster(string pFromDate, string pToDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            bool obj = service.checkDutyRoster(pFromDate, pToDate, pEmployeeId, pDepartmentId, pSectionId);
            return Json(new { data = obj });
        }

        private bool isValid(string pDate, string pFromDate, string pToDate, string pEmployeeId, string pDepartmentId, string pSectionId, string pShiftId)
        {
            if (
                pDate != "" && pFromDate != "" && pToDate != "" && 
                (pEmployeeId != "0" || pEmployeeId != "") && 
                (pDepartmentId != "0" || pDepartmentId != "") && 
                (pSectionId != "0" || pSectionId != "") && 
                (pShiftId != "0" || pShiftId != "")
            )
            {
                return true;
            }
            return false;
        }
    }
}
