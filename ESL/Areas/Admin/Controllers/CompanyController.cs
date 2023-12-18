using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESL.DataAccess;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.TreeMenu;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompanyController> _logger;
        private IBackgroundTaskQueue _backgroundTaskQueue;


        public CompanyController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            ILogger<CompanyController> logger,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        public IActionResult Index()
        {
            ViewBag.isEdit = UserAccessor.isEdit(_userManager,_backgroundTaskQueue, User);
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
            {
                //this is for create
                return View(company);
            }
            //this is for edit
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    company.Persistance = DateTime.Now;
                    company.Status = 0;
                    _unitOfWork.Company.Add(company);
                    
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}