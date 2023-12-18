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
    public class HolidayDeclarationController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<HolidayDeclarationController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        private HolidayDeclarationService service;

        public HolidayDeclarationController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<HolidayDeclarationController>  logger,
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
            service = new HolidayDeclarationService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]        
        public IActionResult HolidaySave(Holiday objHolidayInfo)
        {
            string msg = SD.errorMessage;
            if (isValid(objHolidayInfo))
            {
                msg = SD.successSaveMessage;
                int save = service.HolidaySave(objHolidayInfo);
                if (save > 0)
                {
                    return Json(new { success = true, message = msg });
                }
                else
                {
                    return Json(new { success = false, message = SD.errorMessage });
                }
            }
            return Json(new { success = false, message = msg });
        }
        public IActionResult HolidayDelete(Holiday objHolidayInfo)
        {
            string msg = SD.errorMessage;
            if (isValid(objHolidayInfo))
            {
                msg = SD.successDeleteMessage;
                int save = service.HolidayDelete(objHolidayInfo);
                if (save > 0)
                {
                    return Json(new { success = true, message = msg });
                }
                else
                {
                    return Json(new { success = false, message = SD.errorMessage });
                }
            }
            return Json(new { success = false, message = msg });
        }
        private bool isValid(Holiday objHolidayInfo)
        {
            if (objHolidayInfo.dDate.ToString() != "")
            {
                return true;
            }
            return false;
        }

        #region API CALLS
        public IActionResult FindData(string dFindDate)
        {
            var obj = service.getFindData(dFindDate);
            if (obj != null)
            {
                return Json(new { isFind = true, data = obj });
            }
            return Json(new { isFind = false });
        }
        public IActionResult getAll()
        {
            var obj = service.getAll();
            if (obj != null)
            {
                return Json(new {data = obj });
            }
            return Json(new {});
        }

        #endregion
    }
}
