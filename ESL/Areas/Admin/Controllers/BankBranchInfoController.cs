using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

namespace ESL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class BankBranchInfoController : Controller
    {
        private readonly ILogger<BankInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
       
        public FileUpload _fileUpload;
        public BankBranchInfoController
        (
            IHttpContextAccessor accessor,
            ILogger<BankInfoController>  logger,
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            IWebHostEnvironment hostEnvironment
        )
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _logger = logger;
            _db = db;         
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            BankBranch obj = new BankBranch();
            return View(obj);

        }
        public IActionResult bankBranchSave(BankBranch obj)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if ( !string.IsNullOrEmpty(obj.BranchName))
            {
                                         
                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                obj.EntryTime = DateTime.Now;
              
                
                if (obj.Id == 0)
                {
                    msg = "Information save successfully!";
                    _unitOfWork.BankBranch.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = "Information update successfully!";
                    _unitOfWork.BankBranch.Update(obj);
                }
                _unitOfWork.Save();
                return Json(new { success=true,isUpdate=isUpdate,message=msg});
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
       
       
        public IActionResult nameCheck(string name)
        {
            var obj = _unitOfWork.BankBranch.GetFirstOrDefault(x => x.BranchName.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult findData(int id)
        {
            var obj = _unitOfWork.BankBranch.GetFirstOrDefault(x=>x.Id==id);
            if (obj != null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new { isFind=false});
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.BankBranch.GetAll();
            return Json(new { data = allObj });
        }
        #endregion
    }
}
