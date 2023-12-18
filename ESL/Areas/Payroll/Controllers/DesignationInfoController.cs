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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESL.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class DesignationInfoController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<DesignationInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";

        public FileUpload _fileUpload;

        private DesignationService service;

        public DesignationInfoController(UnitPayrollInterface unitPayroll,
            IHttpContextAccessor accessor,
            ILogger<DesignationInfoController> logger,
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
            service = new DesignationService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? iAutoId)
        {
            Designation obj = new Designation();
            return View(obj);

        }
        public IActionResult GetMaxDesignation()
        {
            var max = service.getMaxDesignation();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }
        public IActionResult getRank()
        {
            var obj = service.getRank();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult DesignationSave(Designation obj)
        {
            bool isUpdate = false;
            string msg = SD.errorMessage;
            if (!string.IsNullOrEmpty(obj.vDesignationName))
            {
                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.vCompanyId = "1";

                obj.EntryTime = DateTime.Now;


                if (obj.iAutoId == 0)
                {
                    msg = SD.successSaveMessage;
                    _unitPayroll.Designation.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = SD.successEditMessage;
                    _unitPayroll.Designation.Update(obj);
                }
                _unitOfWork.Save();
                return Json(new { success = true, isUpdate = isUpdate, message = msg });
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }

        #region API CALLS
        public IActionResult codeCheck(string name)
        {
            var obj = _unitPayroll.Designation.GetFirstOrDefault(x => x.vDesignationCode.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult nameCheck(string name)
        {
            var obj = _unitPayroll.Designation.GetFirstOrDefault(x => x.vDesignationName.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult nameBanglaCheck(string name)
        {
            var obj = _unitPayroll.Designation.GetFirstOrDefault(x => x.vDesignationNameBangla.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }

        public IActionResult findData(int iAutoId)
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
