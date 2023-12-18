using ESL.DataAccess.Data;
using ESL.DataAccess;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESL.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<MenuViewComponent> _logger;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        List<Menu> parentList;
        List<Menu> menuList;
        public MenuViewComponent(
            ApplicationDbContext db,
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            ILogger<MenuViewComponent> logger,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            string userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x=>x.Id==userId);
            ViewBag.UserId = userId;
           
            if (user != null)
            {
                Menus menus;
                if (user.Email == "admin@esl.com" ||
                    user.Email == "admin@gmail.com" ||
                     user.Email == "admin@bfs.com")
                {
                    menus = AdminMenuGenerate();
                }
                else
                {
                    menus = MenuGenerate(userId);
                }

                _backgroundTaskQueue.addMyMenu(userId, menuList);
                //HttpContext.Session.Clear();
                //SessionExtensions.Set(HttpContext.Session, "myMenuList", menuList);
                return View(menus);
            }
            return null;
        }

        private Menus MenuGenerate(string id)
        {
            Menus menus = TreeManuGenerate();
            parentList = new List<Menu>();
            menuList = new List<Menu>();
            var userAuthentications = _unitOfWork.UserAuthentication.GetAll().Where(e => e.AccessUserId.Equals(id)).OrderBy(e => e.ModuleId);
            if (menus != null)
            {
                foreach (Menu module in menus.Modules)
                {
                    foreach (UserAuthentication userAuthentication in userAuthentications)
                    {
                        if (userAuthentication.ModuleId == module.Id)
                        {
                            module.IsActive = true;
                            break;
                        }
                    }
                    GetChildGenerate(module, userAuthentications);
                }
                parentList.Clear();
            }
            return menus;
        }

        private Menu GetChildGenerate(Menu menu, IEnumerable<UserAuthentication> userAuthentications)
        {
            parentList.Add(menu);
            foreach (Menu subMenu in menu.Menus)
            {
                parentList.Add(subMenu);
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
                        menuList.Add(subMenu);
                        setParentMenuActivate(subMenu);
                        break;
                    }
                }

                GetChildGenerate(subMenu, userAuthentications);
            }
            return menu;
        }

        private void setParentMenuActivate(Menu menu) {
            List<Menu> parentMenu = parentList.Where(x => x.Id.Equals(menu.ParentId)).ToList();
            
            if (parentMenu.Any())
            {
               // _logger.LogInformation("module  " + parentMenu[0].Name);
                if (parentMenu[0].ModuleId != 0)
                {
                   // _logger.LogInformation(parentMenu[0].Name + " " + parentMenu[0].ParentId);
                    parentMenu[0].IsActive = true;
                    if(parentMenu[0].IsReporting)
                        menuList.Add(parentMenu[0]);
                    setParentMenuActivate(parentMenu[0]);
                }
            }
        }

        public static Menus TreeManuGenerate()
        {
            try
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"menuJSON\\menu.json"}");
                var JSON = System.IO.File.ReadAllText(folderDetails);
                //_logger.LogInformation(JSON);
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

        private Menus AdminMenuGenerate()
        {
            Menus menus = TreeManuGenerate();
            menuList = new List<Menu>();
            
            if (menus != null)
            {
                foreach (Menu module in menus.Modules)
                {
                   
                    module.IsActive = true;
                    module.IsAdd = true;
                    module.IsEdit = true;
                    module.IsDelete = true;
                    module.IsPreview = true;
                    menuList.Add(module);
                    GetAdminChildGenerate(module);
                }
                
            }
            return menus;
        }

        private Menu GetAdminChildGenerate(Menu menu)
        {
            foreach (Menu subMenu in menu.Menus)
            {
                subMenu.VId = Guid.NewGuid().ToString();

                subMenu.IsActive = true;
                subMenu.IsAdd = true;
                subMenu.IsEdit = true;
                subMenu.IsDelete = true;
                subMenu.IsPreview = true;
                menuList.Add(subMenu);
                GetAdminChildGenerate(subMenu);
            }
            return menu;
        }


    }
}
