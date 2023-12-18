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
    public class DeleteAttendanceController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<DeleteAttendanceController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private DeleteAttendanceService service;

        public DeleteAttendanceController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<DeleteAttendanceController>  logger,
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
            service = new DeleteAttendanceService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult deleteAttendance(string dDate, string pEmployeeId)
        {
            if (isValid(dDate, pEmployeeId))
            {
                int a = service.deleteAttendance(dDate, pEmployeeId);
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successDeleteMessage });
                }
            }
            return Json(new { success = false, message = SD.errorMessage });
        }
        public IActionResult getDepartment(string dDate)
        {
            var obj = service.getDepartment(dDate);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getSection(string dDate, string pDepartmentId)
        {
            var obj = service.getSection(dDate, pDepartmentId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getEmployee(string dDate, string pDepartmentId,string pSectionId)
        {
            var obj = service.getEmployee(dDate, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        private bool isValid(string dDate, string pEmployeeId)
        {
            if (dDate != "" || pEmployeeId != "")
            {
                return true;
            }
            return false;
        }
    }
}
