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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESL.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class GenerateAttendanceController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<GenerateAttendanceController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private GenerateAttendanceService service;

        public GenerateAttendanceController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<GenerateAttendanceController>  logger,
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
            service = new GenerateAttendanceService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Generate(string pGenerateDate, string pAttendanceDate)
        {
            if (isValid(pGenerateDate, pAttendanceDate))
            {
                int a = service.Generate(pGenerateDate, pAttendanceDate);
                if (a > 0)
                {
                    return Json(new { success = true, message = SD.successSaveMessage });
                }
            }
            return Json(new { success = false, message = SD.errorMessage });
        }
        private bool isValid(string pGenerateDate, string pAttendanceDate)
        {
            if (pGenerateDate != "" && pAttendanceDate != "")
            {
                return true;
            }
            return false;
        }
    }
}
