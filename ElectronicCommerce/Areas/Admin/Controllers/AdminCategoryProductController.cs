using AspNetCoreHero.ToastNotification.Abstractions;
using Castle.Core.Configuration;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/categoryproduct")]

    public class AdminCategoryProductController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCategoryProduct;
        private INotyfService _notyfService;
        private ICategoryProductService _categoryProductService;

        public AdminCategoryProductController(IBaseRepository<CategoryProduct> baseRepoCategoryProduct, ICategoryProductService categoryProductService,INotyfService notyfService)
        {
            _baseRepoCategoryProduct = baseRepoCategoryProduct;
            _categoryProductService = categoryProductService;
            _notyfService = notyfService;
        }


        [Route("index")]
        [Route("")]
        

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.categoryParents = _categoryProductService.findAllParentCategory().ToList();
            ViewBag.categoryProduct = _baseRepoCategoryProduct.GetAll().ToList();
           
            return View("index", new CategoryProduct());
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(CategoryProduct categoryProduct)
        {
            string parent_name="";
            foreach (var item in _baseRepoCategoryProduct.GetAll().ToList())
            {
                if (categoryProduct.ParentId == item.Id)
                {
                    parent_name  = item.Name;
                }
            }
            string rename = parent_name + " " + categoryProduct.Name;
            categoryProduct.Name = rename.ToUpper();
            categoryProduct.Id = "CP" + OrderCode.RandomString(3);
            _baseRepoCategoryProduct.Insert(categoryProduct);
            _baseRepoCategoryProduct.Save();
            return RedirectToAction("index");
        }

        [Route("delete/{id}")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                _baseRepoCategoryProduct.Delete(id);
                _baseRepoCategoryProduct.Save();
                _notyfService.Success("Success", 3);
                return RedirectToAction("index");

            }
            catch (Exception e)
            {
                _notyfService.Error("Fail", 3);
                return RedirectToAction("index");
            }
        }
    }
}
