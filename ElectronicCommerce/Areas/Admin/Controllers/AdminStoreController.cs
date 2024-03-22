using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Areas.Customer.Controllers;
using ElectronicCommerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using ElectronicCommerce.Repositories;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/store")]
    public class AdminStoreController : Controller
    {
        private IStoreService _storeService;
        private IWebHostEnvironment _webHostEnvironment; // !IMPORTANT
        private IBaseRepository<Store> _baseRepository;
        private DatabaseContext _db;


        public AdminStoreController(IStoreService storeService, DatabaseContext db, IBaseRepository<Store> baseRepository, IWebHostEnvironment webHostEnvironment)
        {
            _storeService = storeService;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _baseRepository = baseRepository;
        }
        [Route("index")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.store = _storeService.getAll();
            return View();
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add(Store store, IFormFile photo, IFormFile[] photos)
        {
            store.Id = "SP" + OrderCode.RandomString(3);
            if (photo != null)
            {
                var newFileName = Admin.Helpers.FileHelper.GenerateFileName(photo.ContentType);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "admin/images/store", newFileName);
                using (var fileStream = new FileStream(path, FileMode.Create)) // !IMPORTANT
                {
                    // upload file vao fileStream
                    photo.CopyTo(fileStream);
                }
                store.Image = newFileName;
            }

            _baseRepository.Insert(store);
            _baseRepository.Save();

            if (photos.Length > 0)
            {
                foreach (var item in photos)
                {
                    var newFileName = Admin.Helpers.FileHelper.GenerateFileName(item.ContentType);
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "admin/images/store", newFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create)) // !IMPORTANT
                    {
                        // upload file vao fileStream
                        item.CopyTo(fileStream);
                    }

                    var img = new Image();
                    img.Id = "IM" + OrderCode.RandomString(6);
                    img.NameImages = newFileName;
                    img.ProductId = store.Id;
                    _db.Images.Add(img);
                }
                _db.SaveChanges();
            }
            return RedirectToAction("index");

        }
    }
}
