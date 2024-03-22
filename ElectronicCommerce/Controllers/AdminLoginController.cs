using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Controllers
{
    [Route("UserLogin")]
    public class AdminLoginController : Controller
    {
        private INotyfService _notyfService;
        private IUserService _userService;

        public AdminLoginController(INotyfService notyfService, IUserService userService)
        {
            _notyfService = notyfService;
            _userService = userService;
        }

        // GET: /<controller>/
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("login", new User());
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            if (_userService.checkUsernameExists(username))
            {
                if (_userService.checkValidUser(username, password))
                {
                    // Tao admin session
                    var user = new User();
                    HttpContext.Session.SetString("admin", JsonConvert.SerializeObject(user));
                    return RedirectToAction("index", "order", new { area = "admin" });
                }
                else
                {
                    _notyfService.Error("Mật khẩu không khớp, vui lòng kiểm tra lại", 3);
                    return View("login", new User());
                }
            }
            else
            {
                _notyfService.Error("Tài khoản này không tồn tại", 3);
                return View("login", new User());
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            // Xoa session
            HttpContext.Session.Remove("admin");
            return RedirectToAction("login");
        }
    }
}
