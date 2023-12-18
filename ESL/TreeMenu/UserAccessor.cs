using ESL.DataAccess;
using ESL.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;


namespace ESL.TreeMenu
{

    public class UserAccessor
    {
       

        public static Menu getMyActiveMenu(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu;
        }

        public static bool isActive(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu.IsActive;
        }
        public static bool isAdd(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu.IsAdd;
        }

        public static bool isEdit(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu.IsEdit;
        }

        public static bool isDelete(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu.IsDelete;
        }

        public static bool isPreview(UserManager<IdentityUser> userManager,
           IBackgroundTaskQueue backgroundTaskQueue,
            System.Security.Claims.ClaimsPrincipal User)
        {
            string userId = userManager.GetUserId(User);
            Menu menu = backgroundTaskQueue.getMyActiveMenu(userId);
            return menu.IsPreview;
        }

    }
}
