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

namespace ESLPOS.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuPushController : ControllerBase
    {
       
        private IBackgroundTaskQueue _backgroudQueue;
        private readonly ILogger<MenuPushController> _logger;
        public MenuPushController(
            IBackgroundTaskQueue backgroudQueue, 
            ILogger<MenuPushController> logger
            )
        {
            this._backgroudQueue = backgroudQueue;
            this._logger = logger;
        }



        [HttpPost]
        public async Task<IActionResult> addMenu(JsonData data)
        {
            Menu menu = _backgroudQueue.getMyMenu(data.uId,Convert.ToInt32(data.menuVId));
            _backgroudQueue.addMyActiveMenu(data.uId, menu);
            return CreatedAtAction("success",menu);
        }

       
    }
}
