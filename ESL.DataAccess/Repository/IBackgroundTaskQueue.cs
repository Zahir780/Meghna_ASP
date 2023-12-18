using ESL.Models;
using ESL.Models.ViewModels;
using ESL.Utility;
using System;
using System.Collections.Generic;
using System.Text;


namespace ESL.DataAccess
{
    public interface IBackgroundTaskQueue
    {
        void setData(SyncUserData userData);
        void addLogedUser(string user);
        void removeLogedUser(string user);
        void addTempData(string user);

        // user Authentication Menus
        Menus getUserMenus(string user);
        void addUserMenu(string user, Menus menus);
        void removeUserMenu(string user);

        // my Menu List
        Menu getMyMenu(string user,int menuId);
        void addMyMenu(string user, List<Menu> menus);
        void removeMyMenu(string user);

        // my Active Menu
        Menu getMyActiveMenu(string user);
        void addMyActiveMenu(string user, Menu menu);
        void removeMyActiveMenu(string user);

        // addSales
        void salesRenduring(DateTime time);
        void clearSells();

        //Dashboard
        SyncUserData getDashboardData();
        void setDashboardData(SyncUserData syncUserModel);

        List<ApplicationUser> getUsers();
        void setUsers(List<ApplicationUser> users);

        void setChatMessage(string userId, ChatMessage chat);

        SyncUserData getChatMessage(string userId);
        List<string> getOnlineUsers();
    }
}
