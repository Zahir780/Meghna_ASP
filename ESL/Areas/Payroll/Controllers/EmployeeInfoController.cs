using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Services.Payroll;
using ESL.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ESL.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    /*[Authorize]*/
    public class EmployeeInfoController : Controller
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _hostEnvironment;
        ILogger<EmployeeService> _logger;
        private ApplicationDbContext _db;
        EmployeeService service;
        public FileUpload _fileUpload;
        public EmployeeInfoController
        (
            UnitPayrollInterface unitPayroll,
            IUnitOfWork unitOfWork,
            ILogger<EmployeeService> logger,
            ApplicationDbContext db,
            IHttpContextAccessor accessor,
            IWebHostEnvironment hostEnvironment
        )
        {
            _unitOfWork = unitOfWork;
            _unitPayroll = unitPayroll;
            _accessor = accessor;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _db = db;
            service = new EmployeeService(_unitPayroll, _unitOfWork, _accessor, logger, _db);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult employeeSave(Employee objEmployeeInfo, List<EmpEducation> objEducation, List<EmpExperience> objExperience, bool addUpdate, string attachment)
        /*public IActionResult employeeSave(Employee objEmployeeInfo, bool addUpdate)*/
        {
            string msg = SD.errorMessage;
            if (isValid(objEmployeeInfo))
            {
                msg = SD.successSaveMessage;
                if (addUpdate)
                {
                    msg = SD.successEditMessage;
                }
                attachment = setAttachment(objEmployeeInfo.vEmployeePhoto, attachment);

                int save = service.employeeSave(objEmployeeInfo, objEducation, objExperience, addUpdate, attachment);
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
        private string setAttachment(string vEmployeePhoto, string Attachment)
        {
            _fileUpload = new FileUpload(_hostEnvironment, _accessor);

            string fileName = "EMP-" + DateTime.Now.Ticks.ToString();

            string files = _fileUpload.getUploadUrl(Attachment, @"images\payroll\employee\", @"\images\payroll\employee\", fileName, "");

            if (string.IsNullOrEmpty(files))
            {
                if (vEmployeePhoto != "")
                {
                    if (Attachment == "" || Attachment == null)
                    {
                        files = "";
                    }
                    else
                    {
                        files = Attachment;
                    }
                }
                else
                {
                    files = "";
                }
            }
            Attachment = files;
            return Attachment;
        }
        private bool isValid(Employee objEmployeeInfo)
        {
            if (objEmployeeInfo.vEmployeeId != "" /* && objEmployeeInfo.vEmpName!="" && objEmployeeInfo.vEmpType!=""*/)
            {
                return true;
            }
            return false;
        }

        #region API CALLS
        public IActionResult GetMaxEmployee()
        {
            var max = service.getMaxEmployee();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }
        public IActionResult getMaxEmployeeCode()
        {
            var max = service.getMaxEmployeeCode();
            if (max != null)
            {
                return Json(new { data = max });
            }
            return Json(new { });
        }
        public IActionResult employeeCodeExist(string data)
        {
            bool obj = service.employeeCodeExist(data);
            return Json(new { data = obj });
        }
        public IActionResult getDesignation()
        {
            var obj = service.getDesignation();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
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
        public IActionResult getSection()
        {
            var obj = service.getSection();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getBank()
        {
            var obj = service.getBank();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult getBankBranch()
        {
            var obj = service.getBankBranch();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }

        public IActionResult findData(string iAutoId)
        {
            var obj=service.findAll(iAutoId);
            if (obj != null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new { isFind=false});
        }
        public IActionResult findEducationData(string vEmployeeId)
        {
            var obj=service.findEducationData(vEmployeeId);
            if (obj != null)
            {
                return Json(new { isFind = true, data = obj });
            }
            return Json(new { isFind = false });
        }
        public IActionResult findExperienceData(string vEmployeeId)
        {
            var obj=service.findExperienceData(vEmployeeId);
            if (obj != null)
            {
                return Json(new { isFind = true, data = obj });
            }
            return Json(new { isFind = false });
        }

        /*[HttpGet]*/
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
