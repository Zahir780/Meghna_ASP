using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESL.DataAccess;
using ESL.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ESL.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private IBackgroundTaskQueue _backgroundQueue;
        private readonly ESL.DataAccess.Repository.UnitOfWork _unitOfWork;
       
        private IHttpContextAccessor _accessor;

        public LogoutModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LogoutModel> logger,
            IBackgroundTaskQueue backgroundQueue,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _backgroundQueue = backgroundQueue;
            _unitOfWork = unitOfWork as ESL.DataAccess.Repository.UnitOfWork;
            _accessor = accessor;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            string userid = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userid);
            _backgroundQueue.removeLogedUser(userid);
            _backgroundQueue.removeMyActiveMenu(userid);
            _backgroundQueue.removeMyMenu(userid);
            _backgroundQueue.removeUserMenu(userid);
            await _signInManager.SignOutAsync();
            
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
