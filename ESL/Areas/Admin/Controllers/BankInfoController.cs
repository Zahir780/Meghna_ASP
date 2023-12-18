using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
    public class BankInfoController : Controller
    {
        private readonly ILogger<BankInfoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        String sqlCon = "";
        BankService service;
        public FileUpload _fileUpload;
        public BankInfoController
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
            service = new BankService(_unitOfWork, _accessor,_db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Bank obj = new Bank();
            return View(obj);

        }
        public IActionResult bankSave(Bank obj, IList<IFormFile> file)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if ( !string.IsNullOrEmpty(obj.BankName))
            {
               
                var objDb = _unitOfWork.Bank.Get(obj.Id);
               var dbImage = "";
               
                
                if (objDb!=null)
                {
                    dbImage = objDb.BankLogo;
                }

                string fileName = "Banklogo_" + obj.BankName.ToString();
                #region Image Upload 
                _fileUpload = new FileUpload(_hostEnvironment, _accessor);
                #endregion
             //   string fileName = "Banklogo_" + DateTime.Now.ToString("dd/MM/yyyy") ;


                string image = _fileUpload.getUploadUrl(obj.BankLogo,
                        @"images\bank", @"\images\bank\", fileName, file);
                if (string.IsNullOrEmpty(image))
                {
                    image = dbImage;
                }
                obj.BankLogo = image;

                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.EntryTime = DateTime.Now;
                if (obj.Id == 0)
                {
                    msg = "Information save successfully!";
                    _unitOfWork.Bank.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = "Information update successfully!";
                    _unitOfWork.Bank.Update(obj);
                }
                _unitOfWork.Save();
                return Json(new { success=true,isUpdate=isUpdate,message=msg});
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
       
       
        public IActionResult nameCheck(string name)
        {
            var obj = _unitOfWork.Bank.GetFirstOrDefault(x => x.BankName.Equals(name));

            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult findData(int id)
        {
            var obj = _unitOfWork.Bank.GetFirstOrDefault(x=>x.Id==id);
            if (obj != null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new { isFind=false});
        }
        #region Table data 
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Bank.GetAll();
            return Json(new { data = allObj });
        }
        #endregion
    }
}
