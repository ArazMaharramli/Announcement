using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Auth.ViewModels.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Area("Auth")]
    [Route("[area]/[action]")]
    public class AccountController : Controller
    {
        /*private readonly IUserManagerService _userManagerService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IUserManagerService userManagerService, IEmailService emailService, SignInManager<ApplicationUser> signInManager)
        {
            _userManagerService = userManagerService;
            _emailService = emailService;
            _signInManager = signInManager;
        }
        */
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }
        /*
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (result.Succeeded)
            {
                //add check activity url
                //await _emailService.SendLoginAttemptEmailAsync(model.Email, response.User.FullName, "google.com");

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Email or password is incorrect");
            return View(model);
        }
        */
        public IActionResult Register()
        {
            return View();
        }
        /*
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = await _userManagerService.CreateUserAsync(
                    model.Email,
                    model.Password
                );

            if (response.Result.Succeeded)
            {
                await SendConfirmationCode(response.User.Id);

                return RedirectToAction("ConfirmEmail", "Account", new { id = response.User.Id });
            }
            foreach (var error in response.Result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(model);
        }
        */

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string id)
        {
            var model = new ConfirmEmailViewModel
            {
                Id = id
            };
            return View(model);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailViewModel model)
        {
            var response = await _userManagerService.ConfirmEmail(model.Id, model.Code);
            if (response.Result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in response.Result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(model);
        }
        */
        public IActionResult Logout()
        {
            return View();
        }

        public async Task<IActionResult> ResendConfirmationCodeAsync(string Id)
        {
            await SendConfirmationCode(Id);
            return Ok();
        }
        public IActionResult ExternalLogin()
        {
            return View();
        }

        #region MyRegion
        private async Task SendConfirmationCode(string userId)
        {

        }
        #endregion
    }
}
