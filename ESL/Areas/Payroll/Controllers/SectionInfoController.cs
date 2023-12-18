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
    public class SectionInfoController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<SectionInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
       
        public FileUpload _fileUpload;

        private SectionService service;

        public SectionInfoController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<SectionInfoController>  logger,
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
            service = new SectionService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? iAutoId)
        {
            Section obj = new Section();
            return View(obj);

        }
        public IActionResult GetMaxSection()
        {
            var max = service.getMaxSection();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }

        public IActionResult SectionSave(Section obj)
        {
            bool isUpdate = false;
            string msg = SD.errorMessage;
            if ( !string.IsNullOrEmpty(obj.vSectionName))
            {
                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.vCompanyId ="1";

                obj.EntryTime = DateTime.Now;
              
                
                if (obj.iAutoId == 0)
                {
                    msg = SD.successSaveMessage;
                    _unitPayroll.Section.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = SD.successEditMessage;
                    _unitPayroll.Section.Update(obj);
                }
                _unitOfWork.Save();
                return Json(new { success=true,isUpdate=isUpdate,message=msg});
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }

        #region API CALLS
        public IActionResult codeCheck(string name)
        {
            var obj = _unitPayroll.Section.GetFirstOrDefault(x => x.vSectionCode.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult nameCheck(string name)
        {
            var obj = _unitPayroll.Section.GetFirstOrDefault(x => x.vSectionName.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult nameBanglaCheck(string name)
        {
            var obj = _unitPayroll.Section.GetFirstOrDefault(x => x.vSectionNameBangla.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }

        public IActionResult findData(int iAutoId)
        {
            var obj=service.finAllData(iAutoId);
            if (obj != null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new { isFind=false});
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
