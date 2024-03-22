using AspNetCoreHero.ToastNotification.Abstractions;
using Castle.Core.Configuration;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/product")]


    public class AdminProductController : Controller
    {
        private IBaseRepository<Product> _baseRepoProduct;
        private IBaseRepository<CategoryProduct> _baseRepoCategory;
        private ICategoryProductService _categoryProductService;
        private IBaseRepository<StoneType> _baseRepoStoneType;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private INotyfService _notyfService;
        private IBaseRepository<ProductDetail> _baseRepoProductDetail;
        private IBaseRepository<ProductPrice> _baseRepoProductPrice;
        private DatabaseContext _db;

        private IWebHostEnvironment _webHostEnvironment; // !IMPORTANT

        public AdminProductController(IBaseRepository<Product> baseRepoProduct, INotyfService notyfService,
            ICategoryProductService categoryProductService, IBaseRepository<StoneType> baseRepoStoneType,
            IBaseRepository<Geomancy> baseRepoGeomancy, IBaseRepository<CategoryProduct> baseRepoCategory,
             IBaseRepository<ProductDetail> baseRepoProductDetail, IBaseRepository<ProductPrice> baseRepoProductPrice,
            IWebHostEnvironment webHostEnvironment, DatabaseContext db)
        {
            _baseRepoProduct = baseRepoProduct;
            _notyfService = notyfService;
            _categoryProductService=categoryProductService;
            _baseRepoStoneType = baseRepoStoneType;
            _baseRepoGeomancy = baseRepoGeomancy;
            _baseRepoCategory = baseRepoCategory;
            _baseRepoProductDetail = baseRepoProductDetail;
            _baseRepoProductPrice = baseRepoProductPrice;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }


        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var product = _baseRepoProduct.GetAll().ToList();
            ViewBag.products = product;
            ViewBag.stoneTypes = _baseRepoStoneType.GetAll().ToList();
            ViewBag.Categories = _baseRepoCategory.GetAll().ToList().Where(i => i.ParentId != null).ToList();
            ViewBag.Geomancies = _baseRepoGeomancy.GetAll().ToList();
            return View("index");
        }
        // cap nhat bestseller
        [Route("updatebestseller")]
        [HttpPost]
        public IActionResult UpdateBestSeller(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if(product.BestSeller)
            {
                product.BestSeller = false;
            }
            else
            {
                product.BestSeller = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new {message="Success"});
        }
        //cajp nhat homeflag
        [Route("updatehomeflag")]
        [HttpPost]
        public IActionResult UpdateHomeFlag(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if (product.HomeFlag)
            {
                product.HomeFlag = false;
            }
            else
            {
                product.HomeFlag = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new { message = "Success" });
        }
        // cap nhat trang thai
        [Route("updateactive")]
        [HttpPost]
        public IActionResult UpdateActive(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if (product.Active)
            {
                product.Active = false;
            }
            else
            {
                product.Active = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new { message = "Success" });
        }

        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            // hien thi thong tin san pham trong trang detail
            var commonProductModel = new CommonProductModel();
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(id));
            commonProductModel.PRODUCT = product;
            ViewBag.product = product;
            //lay danh sach chi tiet san pham
            List<ProductDetail> productDetails = _baseRepoProductDetail.GetAll().ToList().Where(i => i.ProductId.Equals(id)).ToList(); ;
            ViewBag.productDetails = productDetails;
            // selection 
            ViewBag.categories = _baseRepoCategory.GetAll().ToList().Where(i => i.ParentId != null).ToList();
            ViewBag.geomancies = _baseRepoGeomancy.GetAll().ToList();
            ViewBag.stoneTypes = _baseRepoStoneType.GetAll().ToList();
            return View("detail", commonProductModel);
        }
        // chinh sua san pham
        [HttpPost]
        [Route("update")]
        public IActionResult Update(CommonProductModel commonProduct)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(commonProduct.PRODUCT.Id));
            product.Name = commonProduct.PRODUCT.Name;
            product.CatId = commonProduct .PRODUCT.CatId;
            product.MainStoneId = commonProduct.PRODUCT.MainStoneId;
            product.SubStoneId = commonProduct.PRODUCT.SubStoneId;
            product.GeomancyId = commonProduct.PRODUCT.GeomancyId;
            product.Color = commonProduct.PRODUCT.Color;
            product.Note = commonProduct.PRODUCT.Note;
            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();

            return RedirectToAction("detail", new { id = product.Id });
        }
        // them chi tiet san pham
        [HttpPost]
        [Route("addProductDetail")]
        public IActionResult addProductDetail(int size, int import_quantity, int base_price, int sale_price, string product_id)
		{
            ProductPrice productPrice = new ProductPrice();
            productPrice.ProductPriceId = "PP" + OrderCode.RandomString(3);
            productPrice.BasePrice = base_price;
            productPrice.SalePrice = sale_price;
            productPrice.CreatedDate= DateTime.Now;
            productPrice.InActive = true;
            _baseRepoProductPrice.Insert(productPrice);
            _baseRepoProductPrice.Save();
            ProductDetail productDetail = new ProductDetail();
            productDetail.ProductDetailId = "PD" + OrderCode.RandomString(3);
            productDetail.ProductId = product_id;
            productDetail.Quantity = import_quantity;
            productDetail.ImportQuantity = import_quantity;
            productDetail.ProductPriceId = productPrice.ProductPriceId;
            productDetail.CreatedDate = DateTime.Now;
            productDetail.Size = size;
            productDetail.ProductId = product_id;
            _baseRepoProductDetail.Insert(productDetail);
            _baseRepoProductDetail.Save();

            return RedirectToAction("detail", new { id=product_id });

        }


        [HttpPost]
        [Route("importQuantity")]
        public IActionResult importQuantity(string id, int import_quantity)
        {
            var productDetail = _baseRepoProductDetail.GetAll().ToList().SingleOrDefault(p => p.ProductDetailId.Equals(id));
            productDetail.ImportQuantity = import_quantity + productDetail.ImportQuantity;
            productDetail.Quantity = productDetail.Quantity + import_quantity;
            _baseRepoProductDetail.Update(productDetail);
            _baseRepoProductDetail.Save();
            return RedirectToAction("detail", new { id = productDetail.ProductId });

        }

        [Route("deleteProductDetail/{id}")]
        public IActionResult deleteProductDetail(string id)
        {
            var productDetail = _baseRepoProductDetail.GetAll().ToList().SingleOrDefault(p => p.ProductDetailId.Equals(id));
            _baseRepoProductDetail.Delete(productDetail.ProductDetailId);
            _baseRepoProductDetail.Save();
            return RedirectToAction("detail", new { id = productDetail.ProductId });

        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(Product product, IFormFile photo, IFormFile[] photos)
        {
            product.Id = "SP" + OrderCode.RandomString(3);
            product.BestSeller = false;
            product.Active = false;
            product.HomeFlag = false;
            if (photo != null)
            {
                var newFileName = Admin.Helpers.FileHelper.GenerateFileName(photo.ContentType);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "admin/images/products", newFileName);
                using (var fileStream = new FileStream(path, FileMode.Create)) // !IMPORTANT
                {
                    // upload file vao fileStream
                    photo.CopyTo(fileStream);
                }
                product.Image = newFileName;
            }

            _baseRepoProduct.Insert(product);
            _baseRepoProduct.Save();

            if (photos.Length >0)
            {
                foreach (var item in photos)
                {
                    var newFileName = Admin.Helpers.FileHelper.GenerateFileName(item.ContentType);
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "admin/images/products", newFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create)) // !IMPORTANT
                    {
                        // upload file vao fileStream
                        item.CopyTo(fileStream);
                    }

                    var img = new Image();
                    img.Id = "IM" + OrderCode.RandomString(6);
                    img.NameImages = newFileName;
                    img.ProductId = product.Id;
                    _db.Images.Add(img);
                }
                _db.SaveChanges();
            }
            return RedirectToAction("index");
        }


        
    }
}
