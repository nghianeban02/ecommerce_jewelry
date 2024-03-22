using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/user")]
    public class AdminUserController : Controller
    {
        // GET: /<controller>/
        private IConfiguration _configuration;
        private IBaseRepository<User> _baseRepoUser;
        private IBaseRepository<Role> _baseRepoRole;
        private IUserService _userService;
        private INotyfService _notyfService;

        public AdminUserController(IBaseRepository<User> baseRepoUser, IUserService userService,
            IBaseRepository<Role> baseRepoRole, INotyfService notyfService, IConfiguration configuration)
        {
            _baseRepoRole = baseRepoRole;
            _baseRepoUser = baseRepoUser;
            _userService = userService;
            _notyfService = notyfService;
            _configuration = configuration;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.roles = _baseRepoRole.GetAll().ToList();
            ViewBag.users = _baseRepoUser.GetAll().ToList();
            Debug.WriteLine("!@#AWQ: "+_baseRepoRole.GetAll().ToList().Count);
            return View("index", new User());
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Update(string id)
        {
            var user = _baseRepoUser.GetById(id);
            ViewBag.roles = _baseRepoRole.GetAll().ToList();
            return View("update", user);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(User user)
        {
            try
            {
                _baseRepoUser.Update(user);
                _baseRepoUser.Save();
                _notyfService.Success("Success", 3);
                return RedirectToAction("index");
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                _notyfService.Error("Fail", 3);
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(User user)
        {
            try
            {
                // format dob and reassign dob for user 
                string date = user.Dob.ToString("dd-MM-yyyy");
                DateTime dobFormatted = DateTime.ParseExact(date, "dd-MM-yyyy",
                                         System.Globalization.CultureInfo.InvariantCulture);
                user.Dob = dobFormatted;
                if (ModelState.IsValid)
                {
                    user.Id = "US" + OrderCode.RandomString(3);

                    // Tao password otp
                    Random rnd = new Random();

                    // Gui mail random password cho nhan vien
                    var randomPassword = rnd.Next(100000, 999999);

                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword.ToString());

                    // Set otp for user
                    user.Password = passwordHash;
                    // Gui otp qua mail
                    var mailHelper = new MailHelper(_configuration);
                    string subject = "Mail gửi với mục đích kích hoạt tài khoản và thông tin về mật khẩu";
                    string content = "Sau khi nhận mật khẩu hãy đăng nhập và thay đổi mật khẩu. Mật khẩu của bạn là: " + randomPassword;
                    if (mailHelper.Send("tung.lt712@gmail.com", user.Username, subject, content))
                    {
                        Debug.WriteLine("Send mail success");
                    }
                    else
                    {
                        Debug.WriteLine("Send mail fail");
                    }

                    _baseRepoUser.Insert(user);
                    _baseRepoUser.Save();
                    _notyfService.Success("Success", 3);
                    return RedirectToAction("index");
                }
                else
                {
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                    _notyfService.Error("Fail", 3);
                    return RedirectToAction("index");
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                _notyfService.Error("Fail", 3);
                return RedirectToAction("index");
            }
        }

        [Route("findAllUserByRole/{id}")]
        public IActionResult findAllUserByRole(string id)
        {
            ViewBag.users = _userService.findAllUserByRole(id);
            ViewBag.roles = _baseRepoRole.GetAll().ToList();
            return View("index");
        }

        [Route("filterUserByRole")]
        [HttpPost]
        public IActionResult filterUserByRole(string role_id)
        {
            ViewBag.users = _userService.findAllUserByRole(role_id);
            ViewBag.roles = _baseRepoRole.GetAll().ToList();
            return View("index");
        }
    }
}
