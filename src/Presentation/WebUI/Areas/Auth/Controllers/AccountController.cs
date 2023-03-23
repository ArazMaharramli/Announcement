﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Users.Commands.ConfirmEmail;
using Application.CQRS.Users.Commands.Register;
using Application.CQRS.Users.Commands.SendEmailConfirmationCode;
using Application.CQRS.Users.Queries.FindByEmail;
using Infrastructure.Identity.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Auth.ViewModels.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Area("Auth")]
    [Route("[area]/[action]/{id?}")]
    [Route("{lang}[area]/[action]/{id?}")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediatr;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, IMediator mediatr, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _mediatr = mediatr;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(FindUserByEmailQuery model, string returnUrl, CancellationToken cancellationToken)
        {
            var res = await _mediatr.Send(model, cancellationToken);
            await _mediatr.Publish(new SendEmailConfirmationCodeCommand { UserId = res.Id }, cancellationToken);
            return RedirectToAction("ConfirmEmail", "Account", new { id = res.Id, returnUrl = returnUrl });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterUserCommand model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _mediatr.Send(model);

            return RedirectToAction("ConfirmEmail", "Account", new { id = response.Id });

        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string id, string returnUrl)
        {
            var model = new ConfirmEmailCommand
            {
                UserId = id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailCommand command, string returnUrl)
        {
            var response = await _mediatr.Send(command);
            if (response.Succeeded)
            {
                await _signInManager.SignInAsync(await _userManager.FindByIdAsync(command.UserId), true);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Rooms", new
                    {
                        Area = ""
                    });
                }
                return LocalRedirect(returnUrl);
            }
            foreach (var error in response.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Rooms", new { Area = "" });
        }

        public async Task<IActionResult> ResendConfirmationCodeAsync(string Id)
        {
            await _mediatr.Publish(new SendEmailConfirmationCodeCommand { UserId = Id });
            return Ok();
        }

        public IActionResult ExternalLogin()
        {
            return View();
        }
    }
}
