using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESLPOS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserProfile : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileUpload _fileUpload;
        private IHttpContextAccessor _httpAccessor;
        private ISession _session => _httpAccessor.HttpContext.Session;
        public UserProfile(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            IHttpContextAccessor httpAccessor)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _httpAccessor = httpAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert()
        {
            var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(x=>x.Id==userId);
            if(obj==null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ApplicationUser appUser)
        {
            var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId);
            obj.PhoneNumber = appUser.PhoneNumber;

            _fileUpload = new FileUpload(_hostEnvironment, _httpAccessor);
            string fileName = "PF_"+obj.UserName;

            string image = _fileUpload.getUploadUrl(appUser.ImagePF,
                @"images", @"\images\", fileName, "imageUp");

            if (string.IsNullOrEmpty(image))
            {
                image = obj.ImagePF;
            }
            obj.ImagePF = image;
            _unitOfWork.ApplicationUser.Update(obj);
            _unitOfWork.Save();
            _session.SetString("profilePic", image);
            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }
    }
}
