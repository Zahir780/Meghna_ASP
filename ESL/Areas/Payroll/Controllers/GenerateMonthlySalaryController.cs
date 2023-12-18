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
    public class GenerateMonthlySalaryController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<GenerateMonthlySalaryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private GenerateMonthlySalaryService service;

        public GenerateMonthlySalaryController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<GenerateMonthlySalaryController>  logger,
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
            service = new GenerateMonthlySalaryService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Generate(
            string pGenerateDate, string pSalaryMonthDate, string pEmployeeId, 
            string pDepartmentId, string pSectionId, string pRevenueStamp
        )
        {
            if (isValid(pGenerateDate, pSalaryMonthDate, pEmployeeId, pDepartmentId, pSectionId, pRevenueStamp))
            {
                int a = service.Generate(pGenerateDate, pSalaryMonthDate, pEmployeeId, pDepartmentId, pSectionId, pRevenueStamp);
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
        public IActionResult getEmployee(string pDepartmentId,string pSectionId)
        {
            var obj = service.getEmployee(pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getMonthRelatedData(string pSalaryMonthDate)
        {
            var obj = service.getMonthRelatedData(pSalaryMonthDate);
            if (obj != null)
            {
                return Json(new { data = obj, isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult checkSalary(string pSalaryMonthDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            bool obj = service.checkSalary(pSalaryMonthDate, pEmployeeId, pDepartmentId, pSectionId);
            return Json(new { data = obj });
        }

        private bool isValid(
            string pGenerateDate, string pSalaryMonthDate, string pEmployeeId, string pDepartmentId, string pSectionId, string pRevenueStamp
        )
        {
            if (pGenerateDate != "" && pSalaryMonthDate != "")
            {
                return true;
            }
            return false;
        }
    }
}
