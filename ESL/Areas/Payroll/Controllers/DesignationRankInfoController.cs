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
    public class DesignationRankInfoController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly ILogger<DesignationRankInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
       
        public FileUpload _fileUpload;

        private DesignationRankService service;

        public DesignationRankInfoController(UnitPayrollInterface unitPayroll, 
            IHttpContextAccessor accessor,
            ILogger<DesignationRankInfoController>  logger,
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
            service = new DesignationRankService(_unitPayroll, _unitOfWork, _accessor, _db);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? iAutoId)
        {
            DesignationRank obj = new DesignationRank();
            return View(obj);

        }
        public IActionResult GetMaxDesignationRank()
        {
            var max = service.getMaxDesignationRank();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }

        public IActionResult DesignationRankSave(DesignationRank obj)
        {
            bool isUpdate = false;
            string msg = SD.errorMessage;
            if ( !string.IsNullOrEmpty(obj.iRank.ToString()))
            {
                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.vCompanyId ="1";

                obj.EntryTime = DateTime.Now;
              
                
                if (obj.iAutoId == 0)
                {
                    msg = SD.successSaveMessage;
                    _unitPayroll.DesignationRank.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = SD.successEditMessage;
                    _unitPayroll.DesignationRank.Update(obj);
                }
                _unitOfWork.Save();
                return Json(new { success=true,isUpdate=isUpdate,message=msg});
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }

        #region API CALLS
        public IActionResult nameCheck(string name)
        {
            var obj = _unitPayroll.DesignationRank.GetFirstOrDefault(x => x.iRank.Equals(name));

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
