using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ESL.DataAccess.Data;
using Microsoft.Extensions.Logging;
using ESL.DataAccess;

namespace ESL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private IHttpContextAccessor _accessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<NotificationController> _logger;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        public NotificationController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IHttpContextAccessor accessor, UserManager<IdentityUser> userManager,
            ApplicationDbContext db, ILogger<NotificationController> logger,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _accessor = accessor;
            _userManager = userManager;
            _db = db;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
        }


        [HttpPost]
        public IActionResult Get()
        {
            var allUnreadNotifiy = _db.tbNotification.Where(e => e.UserId.Equals(User.Identity.Name) && e.IsRead.Equals(0));
            //_logger.LogInformation("sddd" +allUnreadNotifiy.Count().ToString());
            foreach (Notification notification in allUnreadNotifiy) {
                notification.IsRead = 1;
            }
            _db.SaveChanges();
            return Json(allUnreadNotifiy);
        }

        [HttpPost]
        public IActionResult Count()
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _logger.LogInformation(userId);
            var allNotify = _unitOfWork.Notification.GetAll().Where(e => e.UserId.Equals(User.Identity.Name) && e.IsRead.Equals(0));
            var allChat = _unitOfWork.ChatMessage.GetAll().Where(e => e.ReceiverId.Equals(userId) && e.IsRead.Equals(false));
            return Json(new {chat = allChat.ToList().Count(), notify = allNotify.ToList().Count() });
        }

        //[HttpPost]
        public JsonResult SendMessage(ChatMessage message)
        {
            //_logger.LogInformation(message.Text + "");
            if (message != null)
            {
                message.Date = DateTime.Now;
                ChatMessage chat = _unitOfWork.ChatMessage.Add(message);
                _unitOfWork.Save();
                _backgroundTaskQueue.setChatMessage(chat.ReceiverId, chat);
                return Json(new { result = true, data = chat });
            }

            return Json(new { result = false });
        }
        public JsonResult GetMessage(string senderId, string reciverId)
        {
            try
            {
                var unread = _unitOfWork.ChatMessage.GetAll
                    (x => x.SenderId.Equals(senderId)
                    && x.ReceiverId.Equals(reciverId)
                    && x.IsRead == false);
                foreach (ChatMessage chat in unread)
                {
                    chat.IsRead = true;
                    _unitOfWork.ChatMessage.Update(chat);
                }
                _unitOfWork.Save();
                var allChat = _unitOfWork.ChatMessage.GetAll().Where(e => e.ReceiverId.Equals(reciverId) && e.IsRead.Equals(false));
                var fromDate = DateTime.Now.AddDays(-365);
                var toDate = DateTime.Now;
                var mychat = _unitOfWork.ChatMessage.GetAll
                   (x => (x.SenderId.Equals(senderId)
                   && x.ReceiverId.Equals(reciverId)) ||
                   (x.SenderId.Equals(reciverId)
                   && x.ReceiverId.Equals(senderId))
                   && (x.Date >= fromDate && x.Date <= toDate)).OrderBy(x => x.Date);
                return Json(new { result = true, data = mychat, count = allChat.ToList().Count() });
            }
            catch (Exception) { }

            return Json(new { result = false });
        }

        [HttpGet]
        public IActionResult GetUnReadChat()
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var chats = _unitOfWork.ChatMessage.GetAllMyChattingUsers(userId);
            return Json(new { data = chats });
        }
    }
}
