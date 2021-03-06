using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Course.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _IdentityService;

        public AuthController(IIdentityService identityService)
        {
            _IdentityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInput input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _IdentityService.SignIn(input);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(string.Empty,x);
                });
                
                return View();
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _IdentityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
