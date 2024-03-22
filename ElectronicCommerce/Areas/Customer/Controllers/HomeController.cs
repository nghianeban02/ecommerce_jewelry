using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/home")]
    public class HomeController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<OrderProduct> _baseOrderProduct;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private IProductService _productService;
        private INotyfService _notyfService;

        public HomeController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IProductService productService, IBaseRepository<OrderProduct> baseOrderProduct, INotyfService notyfService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _productService = productService;
            _baseOrderProduct = baseOrderProduct;
            _notyfService = notyfService;
        }

        // GET: /<controller>/
        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            // Kiem tra session customer
            if(HttpContext.Session.GetString("customer") !=null)
            {
                ViewBag.customerSession = "ok";
            }
            else
            {
                ViewBag.customerSession = null;
            }

            var total = 0;
            var cartQuantity = 0;

            if (HttpContext.Session.GetString("cart") !=null)
            {
                var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.cart = cartSession;
                cartQuantity = cartSession.Count;

                // Tinh tong tien gio hang
                foreach (var item in cartSession)
                {
                    if(item.isCheck)
                    {
                        total += item.price * item.quantity;
                    }
                }
            }
            ViewBag.total = total;
            ViewBag.quantity = cartQuantity;
            ViewBag.homePros = _productService.findAllHomeFlagProducts();
            ViewBag.actiPros = _productService.findAllActiveProducts();
            ViewBag.bestPros = _productService.findAllBestSellerProducts();
            return View();
        }

        [HttpGet]
        [Route("OrderTrack")]
        public IActionResult OrderTrack()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();
            return View("ordertrack");
        }

        [HttpPost]
        [Route("OrderTrack")]
        public IActionResult OrderTrack(string order_id)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            Debug.WriteLine(order_id);
            var order_product = _baseOrderProduct.GetById(order_id);

            if (order_product != null)
            {
                ViewBag.order = order_product;
                Debug.WriteLine(order_product);
                return View("ordertrack");
            }
            else
            {
                _notyfService.Error("Không tìm thấy đơn hàng tương ứng với mã đã nhập !",3);
                return RedirectToAction("ordertrack");
            }
        }
    }
}
