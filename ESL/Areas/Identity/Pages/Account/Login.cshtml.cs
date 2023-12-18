using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ESL.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using ESL.Utility;
using ESL.DataAccess;
using System.IO;
using Newtonsoft.Json;
using ESL.DataAccess.Repository;
using ESL.Models;

namespace ESL.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUnitOfWork _unitOfWork;
        private IBackgroundTaskQueue _backgroundQueue;
        private ISession _session => _accessor.HttpContext.Session;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            IBackgroundTaskQueue backgroundQueue,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _backgroundQueue = backgroundQueue;
            _unitOfWork = unitOfWork;
            _accessor = accessor;

        }

        [BindProperty]
        public InputModel Input { get; set; }
        

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public void setCompanyInfo(string userName)
        {
            var company = _unitOfWork.Company.GetFirstOrDefault();
            _session.SetString("userName", userName.Substring(0, userName.IndexOf("@")));
            _session.SetString("userIp", SD.getIp());
            _session.SetString("developer", "Developed By : E-Vision Software Ltd.  || Helpline : 01755-506044 || www.eslctg.com");
            _session.SetString("phone", company.PhoneNumber);
            _session.SetString("companyId",company.Id.ToString());
            _session.SetString("companyName", company.Name);
            _session.SetString("companyAddress", company.StreetAddress);
        }
        public void setPFImage(string userName)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Email == userName);
            if (user != null)
            {
                _session.SetString("profilePic", user.ImagePF == null ? "" : user.ImagePF);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            _logger.LogInformation("load");


            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, false);
                if (result.Succeeded)
                {
                    setCompanyInfo(user.Email);
                    setPFImage(user.Email);
                    _session.SetString("userId", user.Id);
                    _session.SetString("userEmail", user.Email);
                    setAllUsers();
                    _backgroundQueue.addLogedUser(user.Id);
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void setAllUsers()
        {

            _backgroundQueue.setUsers(_unitOfWork.ApplicationUser.GetAll().ToList());
        }

       

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Page();
        }

        
    }
}
