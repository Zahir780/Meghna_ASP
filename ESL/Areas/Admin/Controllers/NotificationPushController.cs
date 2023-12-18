using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESL.Models;
using ESL.DataAccess;
using Microsoft.Extensions.Logging;
using ESL.Models.ViewModels;
using Newtonsoft.Json;

namespace ESL.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationPushController : ControllerBase
    {
       
        private IBackgroundTaskQueue _backgroudQueue;
        private readonly ILogger<NotificationPushController> _logger;
        public NotificationPushController(
            IBackgroundTaskQueue backgroudQueue, 
            ILogger<NotificationPushController> logger
            )
        {
            this._backgroudQueue = backgroudQueue;
            this._logger = logger;
        }

        [HttpGet]
        public async Task getTask(string id)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            await Task.Delay(TimeSpan.FromSeconds(2));
            var syncData = _backgroudQueue.getChatMessage(id);
            
            
            if (syncData != null)
            {
                string data = JsonConvert.SerializeObject(syncData);
                //_logger.LogInformation("Chat Sync for " + data);

                string message = $"data:{data}\n\n";
                byte[] messagebyte = System.Text.ASCIIEncoding.ASCII.GetBytes(message);
                await Response.Body.WriteAsync(messagebyte, 0, messagebyte.Length);
                await Response.Body.FlushAsync();
            }
            else {
               // _logger.LogInformation("not find data" + data);
                await Response.Body.FlushAsync();
            }
            

        }

        [HttpPost]
        public async Task<IActionResult> addTask(SyncUserData data)
        {
           _logger.LogInformation("User request post data" + data);
           _backgroudQueue.setData(data);
           return  Accepted();
        }

       
    }
}
