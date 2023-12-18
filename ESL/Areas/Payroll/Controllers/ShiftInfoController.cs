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
    public class ShiftInfoController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<ShiftInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";

        public FileUpload _fileUpload;

        private ShiftService service;

        public ShiftInfoController(UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<ShiftInfoController> logger,
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
            service = new ShiftService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? iAutoId)
        {
            Shift obj = new Shift();
            return View(obj);

        }
        public IActionResult GetMaxShift()
        {
            var max = service.getMaxShift();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }

        public IActionResult shiftSave(Shift obj, bool addUpdate)
        {
            string msg = SD.errorMessage;
            if (isValid(obj))
            {
                msg = SD.successSaveMessage;
                if (addUpdate)
                {
                    msg = SD.successEditMessage;
                }

                int save = service.shiftSave(obj, addUpdate);
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
        private bool isValid(Shift obj)
        {
            if (obj.vShiftName != "")
            {
                return true;
            }
            return false;
        }

        #region API CALLS
        public IActionResult nameCheck(string name)
        {
            var obj = _unitPayroll.Shift.GetFirstOrDefault(x => x.vShiftName.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }

        public IActionResult findData(string iAutoId)
        {
            var obj = service.finAllData(iAutoId);
            if (obj != null)
            {
                return Json(new { data = obj, isFind = true });
            }
            return Json(new { isFind = false });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var obj = service.getAllData();
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        #endregion
    }
}
