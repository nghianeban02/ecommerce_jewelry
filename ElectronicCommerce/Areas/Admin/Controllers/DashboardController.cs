using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashboardController : Controller
    {
        private INotyfService _notyfService;
        public DashboardController(INotyfService notyfService)
        {
            _notyfService = notyfService;
        }

        // GET: /<controller>/
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            _notyfService.Information("Welcome back", 3);
            return View();
        }
    }
}
