using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESL.DataAccess;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ESLPOS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        
        public UserController(
            ApplicationDbContext db,
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            ILogger<UserController> logger,
            IBackgroundTaskQueue backgroundTaskQueue)

        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;


        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == id);
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var roleId = userRole.FirstOrDefault(u => u.UserId == id).RoleId;
            objUser.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            ViewBag.Username = objUser.Name;
            ViewBag.UserType = objUser.Role;
            ViewBag.UserId = objUser.Id;
            ViewBag.UserPic = objUser.ImagePF;
            Menus menus = await MenuGenerate(id);
            _backgroundTaskQueue.addUserMenu(id, menus);
            return  View(menus);
        }


        #region API CALLS



        [HttpGet]
        public IActionResult GetAll()
        {
            string userId = _userManager.GetUserId(User);
            var userList = _db.ApplicationUsers.Include(u=>u.Company).Where(e=> !e.Id.Equals(userId)).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                
            }
            return Json(new { data = userList });
        }

      [HttpPost]
      public IActionResult LockUnlock(string id, string status)
        {
            var objFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }


        [HttpPost]
        public IActionResult ChangeMenu([FromBody] JsonData data)
        {
            Menus menus = _backgroundTaskQueue.getUserMenus(data.uId);
            foreach (Menu module in menus.Modules)
            {
                //_logger.LogInformation(module.Name);
                setChildMenu(module, data.menuVId, data.key, data.isActive);
            }
            _backgroundTaskQueue.addUserMenu(data.uId, menus);
            return Json(menus);
        }

        public Menu setChildMenu(Menu menu, string menuVId, string key, bool isActive)
        {
            foreach (Menu submenu in menu.Menus)
            {
                if (submenu.VId != null
                    && submenu.VId.Equals(menuVId))
                {
                    setActionValue(submenu, key, isActive);
                    return menu;
                }
                setChildMenu(submenu, menuVId, key, isActive);
            }
            return menu;
        }

        private void setActionValue(Menu menu, string key, bool isActive)
        {
            switch (key)
            {
                case "menu":
                    menu.IsActive = isActive;
                    break;
                case "add":
                    menu.IsAdd = isActive;
                    break;
                case "edit":
                    menu.IsEdit = isActive;
                    break;
                case "delete":
                    menu.IsDelete = isActive;
                    break;
                case "preview":
                    menu.IsPreview = isActive;
                    break;

            }
        }

        [HttpPost]
        public IActionResult SaveMenu([FromBody] JsonData data)
        {
            Menus menus = _backgroundTaskQueue.getUserMenus(data.uId);
            
            if (menus != null)
            {
               _unitOfWork.UserAuthentication.RemoveRange(_unitOfWork.UserAuthentication.GetAll().Where(e => e.AccessUserId.Equals(data.uId)).ToList());
               _unitOfWork.Save();
            }
            foreach (Menu module in menus.Modules)
            {
                getChildMenu(module, data.uId);
            }
            _backgroundTaskQueue.removeUserMenu(data.uId);
            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Save Successfully" });
        }

        public Menu getChildMenu(Menu menu, string uId)
        {
            foreach (Menu submenu in menu.Menus)
            {
                if (submenu.IsActive)
                {
                   // _logger.LogInformation(submenu.Name);
                    UserAuthentication userAuthentication = new UserAuthentication();
                    userAuthentication.MenuId = submenu.Id;
                    userAuthentication.ParentId = submenu.ParentId;
                    userAuthentication.ModuleId = submenu.ModuleId;
                    userAuthentication.IsActive = submenu.IsActive;
                    userAuthentication.IsAdd = submenu.IsAdd;
                    userAuthentication.IsEdit = submenu.IsEdit;
                    userAuthentication.IsDelete = submenu.IsDelete;
                    userAuthentication.IsPreview = submenu.IsPreview;

                    userAuthentication.AccessUserId = uId;
                    userAuthentication.Date = DateTime.Now;
                    userAuthentication.UserId = User.Identity.Name;
                    userAuthentication.UserIp = SD.getIp();
                    _unitOfWork.UserAuthentication.Add(userAuthentication);
                    _unitOfWork.Save();
                }
                getChildMenu(submenu, uId);

            }
            return menu;
        }

        [HttpPost]
        public IActionResult SaveAllMenu([FromBody] JsonData data)
        {
            Menus menus = _backgroundTaskQueue.getUserMenus(data.uId);
            if (menus != null)
            {
                _unitOfWork.UserAuthentication.RemoveRange(_unitOfWork.UserAuthentication.GetAll().Where(e => e.AccessUserId.Equals(data.uId)).ToList());
                _unitOfWork.Save();
            }
            foreach (Menu module in menus.Modules)
            {
                getChildAllMenu(module, data.uId);
            }
            _backgroundTaskQueue.removeUserMenu(data.uId);
            // return RedirectToAction(nameof(Index));
            return Json(new { success = true, message = "Save Successfully" });
        }

        public Menu getChildAllMenu(Menu menu, string uId)
        {
            foreach (Menu submenu in menu.Menus)
            {
                if (!submenu.Menus.Any())
                {
                    UserAuthentication userAuthentication = new UserAuthentication();
                    userAuthentication.MenuId = submenu.Id;
                    userAuthentication.ParentId = submenu.ParentId;
                    userAuthentication.ModuleId = submenu.ModuleId;
                    userAuthentication.IsActive = true;
                    userAuthentication.IsAdd = true;
                    userAuthentication.IsEdit = true;
                    userAuthentication.IsDelete = true;
                    userAuthentication.IsPreview = true;

                    userAuthentication.AccessUserId = uId;
                    userAuthentication.Date = DateTime.Now;
                    userAuthentication.UserId = User.Identity.Name;
                    userAuthentication.UserIp = SD.getIp();
                    _unitOfWork.UserAuthentication.Add(userAuthentication);
                    _unitOfWork.Save();
                }

                getChildAllMenu(submenu, uId);

            }
            return menu;
        }

        private async Task<Menus>  MenuGenerate(string id)
        {
            Menus menus = TreeManuGenerate();
            var userAuthentications = _unitOfWork.UserAuthentication.GetAll().Where(e => e.AccessUserId.Equals(id)).OrderBy(e => e.ModuleId);
            if (menus != null)
            {
                foreach (Menu module in menus.Modules)
                {
                    GetChildGenerate(module, userAuthentications);
                }
            }
            return menus;
        }

        private Menu GetChildGenerate(Menu menu, IEnumerable<UserAuthentication> userAuthentications)
        {

            foreach (Menu subMenu in menu.Menus)
            {
                subMenu.VId = Guid.NewGuid().ToString();
                foreach (UserAuthentication userAuthentication in userAuthentications)
                {
                    if (userAuthentication.MenuId == subMenu.Id &&
                        userAuthentication.ModuleId == subMenu.ModuleId &&
                        userAuthentication.ParentId == subMenu.ParentId)
                    {
                        subMenu.IsActive = userAuthentication.IsActive;
                        subMenu.IsAdd = userAuthentication.IsAdd;
                        subMenu.IsEdit = userAuthentication.IsEdit;
                        subMenu.IsDelete = userAuthentication.IsDelete;
                        subMenu.IsPreview = userAuthentication.IsPreview;
                    }
                }
                GetChildGenerate(subMenu, userAuthentications);
            }
            
            return menu;
        }

        public  Menus TreeManuGenerate()
        {
            try
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"menuJSON\\menu.json"}");
                var JSON = System.IO.File.ReadAllText(folderDetails);
                //_logger.LogInformation(JSON.ToString());
                Menus jsonObj = JsonConvert.DeserializeObject<Menus>(JSON);
                //_logger.LogInformation("list " + jsonObj.Modules);
                return jsonObj;

            }
            catch (Exception)
            {
                //_logger.LogInformation(e.Message);

            }
            return null;
        }
        #endregion
    }
}