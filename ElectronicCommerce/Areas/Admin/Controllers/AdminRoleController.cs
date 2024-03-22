using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/role")]
    public class AdminRoleController : Controller
    {
        private IRoleService _roleService;
        private IBaseRepository<Role> _baseRepo;
        private INotyfService _notyfService;

        public AdminRoleController(IRoleService roleService, IBaseRepository<Role> baseRepo, INotyfService notyfService)
        {
            _roleService = roleService;
            _baseRepo = baseRepo;
            _notyfService = notyfService;
        }
        // GET: /<controller>/
        [Route("index")]
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.roles = _roleService.findAllRoleWithTotalUser();
            return View();
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.roles = _baseRepo.GetAll().ToList();

            return View("Add", new Role());
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(Role role)
        {
            try
            {
                _baseRepo.Insert(role);
                _baseRepo.Save();
                _notyfService.Success("Success",3);
                return RedirectToAction("add");
            } catch(Exception e)
            {
                _notyfService.Error("Fail", 3);
                Debug.WriteLine(e.Message);
                return View("Add", new Role());
            }
        }

        [Route("delete/{id}")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                _baseRepo.Delete(id);
                _baseRepo.Save();
                _notyfService.Success("Success", 3);
                return RedirectToAction("add");

            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                _notyfService.Error("Fail", 3);
                return RedirectToAction("add");
            }
        }

        [Route("update/{id}")]
        [HttpGet]
        public IActionResult Update(string id)
        {
            var role = _baseRepo.GetById(id);
            return View("update", role);
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Update(Role role)
        {
            try
            {
                if(role !=null)
                {
                    _baseRepo.Update(role);
                    _baseRepo.Save();
                    _notyfService.Success("Success", 3);
                    return RedirectToAction("add");
                }
                else
                {
                    _notyfService.Error("Role is null",3);
                    return View("update");
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                _notyfService.Error("Fail", 3);
                return RedirectToAction("index");
            }
        }

    }
}
