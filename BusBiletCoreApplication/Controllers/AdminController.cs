﻿using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusBiletCoreApplication.Controllers
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IToastNotification _toastNotification;

        public AdminController(ILogger<AdminController> logger, IToastNotification toastNotification)
        {
            _logger = logger;
            _toastNotification = toastNotification;
        }
        [HttpGet]
        public IActionResult login()
        {           
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult>Giris(Admin admin)
        {
          
            Context c=new Context();
            var result= c.adminler.Where(x=>x.adminEmail==admin.adminEmail&&x.adminPassword==admin.adminPassword).SingleOrDefault();
            if (result!=null) {
               
              var claims = new List<Claim> { new Claim(ClaimTypes.Email, admin.adminEmail) };

                var userIdentify = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentify);
              //  await HttpContext.SignInAsync(principal);
                await HttpContext
                    .SignInAsync(
                    principal,
                    new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(1) });
                return RedirectToAction("Index","Firma");
            }
            _toastNotification.AddErrorToastMessage("Kullanıcı adı veya password hatalı");
            TempData["init"] = 1;
            return RedirectToAction("login");
        }
        public async Task<IActionResult> Cikis()
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           
            return RedirectToAction("login");

        }


    }
}
