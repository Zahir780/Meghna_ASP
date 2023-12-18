////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: BackendProcess.cs
//FileType: Visual C# Source file
//Author : Asaduzzaman
//Created On : 02-18-2020
//Copy Rights : Evision Soft. Ltd.
////////////////////////////////////////////////////////////////////////////////////////////////////////


using ESL.DataAccess.Data;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.DataAccess
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ILogger<BackgroundTaskQueue> _logger;
        public Dictionary<string, Queue<ChatMessage>> myChat = new Dictionary<string, Queue<ChatMessage>>();
        public Dictionary<string, List<Menu>> myMenus = new Dictionary<string, List<Menu>>();
        public Dictionary<string, Menu> myActiveMenu = new Dictionary<string, Menu>();
        public Dictionary<string, Menus> userMenus = new Dictionary<string, Menus>();
        public Dictionary<string, SyncUserData> readings = new Dictionary<string, SyncUserData>();
        public Queue<ChatMessage> chatQueue = new Queue<ChatMessage>();
        public SyncUserData dashboardData = new SyncUserData();
        public List<ApplicationUser> users = new List<ApplicationUser>();
        string pathString = "";
        string connection = "";
        private readonly IServiceScopeFactory _scopeFactory;
        public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger,
            IServiceScopeFactory scopeFactory)
        {
            this._logger = logger;
            this._scopeFactory = scopeFactory;
            _logger.LogInformation("backend start ");
            
            //CreateTextFile();
        }

        public SyncUserData getDashboardData()
        {
            return dashboardData;
        }
        public void setDashboardData(SyncUserData syncUserModel)
        {
            dashboardData = syncUserModel;
        }

        public void setData(SyncUserData syncUserModel)
        {
            /*try
            {
                List<string> notificationUsers = syncUserModel.NotifiyUsers;
                if (notificationUsers != null)
                {
                    foreach (string key in notification.Keys)
                    {
                        if (notificationUsers.Contains(key))
                        {
                            Queue<SyncUserData> messageQueue = notification[key];
                            
                            // _logger.LogInformation("User requested data" + notifyMeObj.EmployeeId);
                            messageQueue.Enqueue(data);
                           
                        }
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogInformation("User request exception" + ex);
            }*/
        }

        public void addLogedUser(string user)
        {
            removeLogedUser(user);
            Queue<ChatMessage> chatQueue = new Queue<ChatMessage>();
            myChat.Add(user, chatQueue);

        }

        public void removeLogedUser(string user)
        {
            if (myChat.ContainsKey(user))
            {
                myChat.Remove(user);
            }
        }

        

        public void addTempData(string user)
        {
            /*Queue<String> messageQueue = userSets[user];
            messageQueue.Enqueue(user);
            messageQueue.Enqueue(user);
            messageQueue.Enqueue(user);
            messageQueue.Enqueue(user);
            messageQueue.Enqueue(user);*/
        }

        // Menus Setting
        public Menus getUserMenus(string user)
        {
            if (userMenus.ContainsKey(user))
            {
                return userMenus[user];
            }
            return null;
        }

        public void addUserMenu(string user,Menus menus)
        {
            removeUserMenu(user);
            userMenus.Add(user, menus);

        }

        public void removeUserMenu(string user)
        {
            if (userMenus.ContainsKey(user))
            {
                userMenus.Remove(user);
            }
        }

        public Menu getMyMenu(string user, int menuId)
        {
            if (myMenus.ContainsKey(user))
            {
                return myMenus[user].Find(e => e.Id.Equals(menuId)); ;
            }
            return null;
        }

        public void addMyMenu(string user, List<Menu> menus)
        {
            removeMyMenu(user);
            myMenus.Add(user, menus);
        }

        public void removeMyMenu(string user)
        {
            if (myMenus.ContainsKey(user))
            {
                myMenus.Remove(user);
            }
        }

        public Menu getMyActiveMenu(string user)
        {
            if (myActiveMenu.ContainsKey(user))
            {
                return myActiveMenu[user];
            }
            return null;
        }

        public void addMyActiveMenu(string user, Menu menu)
        {
            removeMyActiveMenu(user);
            myActiveMenu.Add(user, menu);
        }

        public void removeMyActiveMenu(string user)
        {
            if (myActiveMenu.ContainsKey(user))
            {
                myActiveMenu.Remove(user);
            }
        }

        private void ReadTextFile()
        {
            // Open the stream and read it back.    


            string[] lines = File.ReadAllLines(pathString);

            string[] values = lines
                      .Select(line => line.Substring(line.IndexOf(':') + 1))
                      .ToArray();
            KeyValuePair<string, string>[] dataList = lines
          .Select(line => line.Split(new char[] { ':' }, 2))
          .Where(items => items.Length >= 2) // let's filter out lines without colon
          .Select(items => new KeyValuePair<string, string>(items[0], items[1]))
          .ToArray();



            string server = "";
            string user = "";
            string password = "";
            string db = "";

            foreach (var item in dataList)
            {
                if (item.Key == "server")
                    server = item.Value;
                else if (item.Key == "user")
                    user = item.Value;
                else if (item.Key == "pwd")
                    password = item.Value;
                else if (item.Key == "db")
                    db = item.Value.Trim();

            }
            connection = "Server=" + server + ";Database=" + db + ";User ID=" + user + ";Password=" + password + ";Integrated Security=False;Trusted_Connection=False;";

            //saveSales(false,DateTime.Now);
        }

        public void clearSells()
        {
            
        }

        public void salesRenduring(DateTime time)
        {
            try
            {
                //saveSales(time);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Rendaring Exception :" + ex);
            }
        }

        public List<ApplicationUser> getUsers()
        {
            return users;
        }

        public void setUsers(List<ApplicationUser> _users)
        {
            users = _users;
        }

        public void setChatMessage(string userId,ChatMessage chat)
        {
            try
            {
                if (myChat.ContainsKey(userId))
                {
                   Queue<ChatMessage> messageQueue = myChat[userId];
                    // _logger.LogInformation("User requested data" + notifyMeObj.EmployeeId);
                    messageQueue.Enqueue(chat);
                 }
                 
             }
             catch (Exception ex) {
                 _logger.LogInformation("User chat set request exception" + ex);
             }
        }

        public SyncUserData getChatMessage(string userId)
        {
            if (myChat.ContainsKey(userId))
            {
                SyncUserData syncData = new SyncUserData();
                syncData.OnlineUsers = myChat.Keys.ToList();
                Queue<ChatMessage> messageQueue = myChat[userId];
                if (messageQueue.Any())
                    syncData.ChatMessage= messageQueue.Dequeue();
                return syncData;
            }
            return null;
        }

        public List<string> getOnlineUsers()
        {
            return myChat.Keys.ToList();
        }

        public void addReading(SyncUserData data)
        {
            try
            {
                foreach (var key in myChat.Keys)
                {
                    readings.Add(key, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Reading data set request exception" + ex);
            }
        }

        public SyncUserData getReading(string userId)
        {
            if (readings.ContainsKey(userId))
            {
                return readings[userId];
            }
            return null;
        }
    }


   
}
