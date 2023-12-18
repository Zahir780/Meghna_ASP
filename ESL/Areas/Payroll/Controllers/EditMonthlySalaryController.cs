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
    public class EditMonthlySalaryController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<EditMonthlySalaryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private EditMonthlySalaryService service;

        public EditMonthlySalaryController(UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<EditMonthlySalaryController> logger,
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
            service = new EditMonthlySalaryService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetSalary (
            string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId
        )
        {
            var obj = service.GetSalary(pSalaryDate, pEmployeeId, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }

        public IActionResult UpdateSalary(VMSalary objVM)
        {
            if (isValid(objVM.objList, objVM.pSalaryDate, objVM.pEmployeeId, objVM.pDepartmentId, objVM.pSectionId))
            {
                int a = service.updateSalary(objVM.objList, objVM.pSalaryDate, objVM.pEmployeeId, objVM.pDepartmentId, objVM.pSectionId);
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successEditMessage });
                }
            }
            return Json(new { success = false, message = SD.errorMessage });
        }


        public IActionResult getSalaryDate()
        {
            var obj = service.getSalaryDate();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getDepartment(string pSalaryDate)
        {
            var obj = service.getDepartment(pSalaryDate);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getSection(string pSalaryDate, string pDepartmentId)
        {
            var obj = service.getSection(pSalaryDate, pDepartmentId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getEmployee(string pSalaryDate, string pDepartmentId, string pSectionId)
        {
            var obj = service.getEmployee(pSalaryDate, pDepartmentId, pSectionId);
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        private bool isValid(
            string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId
        )
        {
            if (pSalaryDate != "")
            {
                return true;
            }
            return false;
        }
        private bool isValid(
                List<Salary> objList, string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId
            )
        {
            if (!SD.isEmpty(objList[0].vEmployeeId) && !SD.isEmpty(pSalaryDate) && !SD.isEmpty(pEmployeeId) &&
                !SD.isEmpty(pDepartmentId) && !SD.isEmpty(pSectionId))
            {
                return true;
            }
            return false;
        }
    }
}
