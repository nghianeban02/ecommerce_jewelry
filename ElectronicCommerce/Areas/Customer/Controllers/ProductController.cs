using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("customer/product")]
    public class ProductController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private IBaseRepository<ProductDiscount> _baseProductDiscount;
        private IProductService _productService;
        private ICustomerService _customerService;

        public ProductController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IProductService productService, IBaseRepository<ProductDiscount> baseProductDiscount, ICustomerService customerService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _productService = productService;
            _baseProductDiscount = baseProductDiscount;
            _customerService = customerService;
        }
        [Route("index/{id}")]
        [Route("")]
        // GET: /<controller>/
        public IActionResult Index(string id)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            ViewBag.pros = _productService.findAllProductByCategory(id);
            return View();
        }

        [Route("detail/{id}")]
        public IActionResult detail(string id)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            var product_detail_display = _productService.findProductDetailDisplayByProductId(id);
            var product = _productService.findById(id);

            product.countProduct = product.countProduct + 1;
            _productService.Update(product);


            ViewBag.productDetail = product_detail_display;

            // Binh luan ve san pham cua customer

            ViewBag.customerReviews = _productService.findAllReviewById(id);

            // Tao session luu nhung san pham da xem cua khach hang

            if (HttpContext.Session.GetString("viewed") == null)
            {
                var viewed = new List<OverViewProduct>();
                viewed.Add(new OverViewProduct
                {
                    CODE = product_detail_display.CODE,
                    THUMB_NAIL = product_detail_display.THUMB_NAIL,
                    NAME = product_detail_display.NAME,
                    PRICE = product_detail_display.PRICE,
                    PRICE_AFTER_DISCOUNT = product_detail_display.PRICE_AFTER_DISCOUNT,
                    CountProduct = product_detail_display.countProduct

                }) ;
                HttpContext.Session.SetString("viewed", JsonConvert.SerializeObject(viewed));
                var viewedProducts = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));

                ViewBag.viewedProducts = viewedProducts;
            }
            else
            {
                var viewed = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));

                bool isExists = isViewed(id, viewed);

                if (isExists)
                {
                    ViewBag.viewedProducts = viewed;
                }
                else
                {
                    viewed.Add(new OverViewProduct
                    {
                        CODE = product_detail_display.CODE,
                        THUMB_NAIL = product_detail_display.THUMB_NAIL,
                        NAME = product_detail_display.NAME,
                        PRICE = product_detail_display.PRICE,
                        PRICE_AFTER_DISCOUNT = product_detail_display.PRICE_AFTER_DISCOUNT,
                        CountProduct = product_detail_display.countProduct
                    });
                    viewed.Reverse();
                    HttpContext.Session.SetString("viewed", JsonConvert.SerializeObject(viewed));
                    var viewedProducts = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));
                    ViewBag.viewedProducts = viewedProducts;
                }
            }

            return View("detail");
        }

        [HttpPost]
        [Route("findProductById")]
        public IActionResult findProductById(string product_id)
        {
            var product = _productService.findProductById(product_id);

            if(!(product.NAME.StartsWith("Nhẫn")) && !(product.NAME.StartsWith("Vòng")))
            {
                return new JsonResult("free-size");
            }
            else
            {
                return new JsonResult(product);
            }

        }

        [HttpPost]
        [Route("findAllSizeOfProducts")]
        public IActionResult findAllSizeOfProducts(string product_id)
        {
            return new JsonResult(_productService.findAllSizeOfProducts(product_id));
        }

        [HttpPost]
        [Route("findPriceBySizeAndId")]
        public IActionResult findPriceBySizeAndId(int size, string product_id)
        {
            return new JsonResult(_productService.findPriceBySizeAndId(size, product_id));
        }

        [HttpPost]
        [Route("AddToCart")]
        public IActionResult AddToCart(int size, string product_id)
        {
            // Mai lam task them vao gio hang khi khach hang da dang nhap
            ProductDetail product_detail = _productService.findProductDetailByProductIdAndSize(size, product_id);

            int discountValue = 0;

            foreach (var item in _baseProductDiscount.GetAll().ToList())
            {
                if(item.ProductId !=null)
                {
                    if(item.ProductId.Equals(product_id))
                    {
                        discountValue = (int)item.DiscountValue;
                    }
                }
                else if(item.GemId !=null)
                {
                    if (item.GemId.Equals(product_detail.Product.GeomancyId))
                    {
                        discountValue = (int)item.DiscountValue;
                    }
                }
                else if(item.StoneId !=null)
                {
                    if (item.GemId.Equals(product_detail.Product.MainStoneId))
                    {
                        discountValue = (int)item.DiscountValue;
                    }
                }
            }

            int actualPrice = product_detail.ProductPrice.SalePrice;

            int savePrice = 0;

            double disPrice1 = (double)discountValue / (double)100;
            double disPrice2 = (double)(100 - discountValue) / (double)100;

            if (discountValue > 0)
            {
                savePrice = (int)(product_detail.ProductPrice.SalePrice * disPrice1);
                actualPrice = (int)(product_detail.ProductPrice.SalePrice * disPrice2);
            }
            // Kiem tra customer session
            // Kiem tra neu khach hang dang dang nhap vao thuc hien them vao gio hang
            // Kiem tra neu khach hang thanh vien thi them vao cart trong db
            if (HttpContext.Session.GetString("customer") != null)
            {
                // Doc session cua customer
                var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

                int indexCartDB = ExistsInCartDb(product_detail.ProductDetailId, customer.Id);

                if (indexCartDB == -1)
                {
                    _customerService.addToCartInDb(savePrice, true, product_detail.ProductDetailId, customer, 1);
                }
                else
                {
                    _customerService.updateQuantityCartInDb(indexCartDB,"plus");
                }
            }
            // Tien hanh xu ly cart session nhu bth
            // Tao session cart
            if (HttpContext.Session.GetString("cart") == null)
            {
                var cart = new List<Item>();
                var item = new Item
                {
                    product_detail_id = product_detail.ProductDetailId,
                    product_id = product_id,
                    image = product_detail.Product.Image,
                    size = product_detail.Size,
                    name = product_detail.Product.Name,
                    quantity = 1,
                    price = actualPrice,
                    isCheck = true,
                    savePrice = savePrice
                };
                cart.Add(item);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

                var addItem = new Item();
                addItem.product_detail_id = product_detail.ProductDetailId;
                addItem.product_id = product_id;
                addItem.image = product_detail.Product.Image;
                addItem.size = product_detail.Size;
                addItem.name = product_detail.Product.Name;
                addItem.quantity = 1;
                addItem.price = actualPrice;
                addItem.isCheck = true;
                addItem.savePrice = savePrice;

                return new JsonResult(addItem);
            }
            else
            {
                var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

                int index = Exists(product_detail.ProductDetailId, cart);

                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        product_detail_id = product_detail.ProductDetailId,
                        product_id = product_id,
                        image = product_detail.Product.Image,
                        size = product_detail.Size,
                        name = product_detail.Product.Name,
                        quantity = 1,
                        price = actualPrice,
                        isCheck = true,
                        savePrice = savePrice
                    });
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

                    var addItem = new Item();
                    addItem.product_detail_id = product_detail.ProductDetailId;
                    addItem.product_id = product_id;
                    addItem.image = product_detail.Product.Image;
                    addItem.size = product_detail.Size;
                    addItem.name = product_detail.Product.Name;
                    addItem.quantity = 1;
                    addItem.price = actualPrice;
                    addItem.isCheck = true;
                    addItem.savePrice = savePrice;

                    return new JsonResult(addItem);
                }
                else
                {
                    if(cart[index].quantity >= product_detail.Quantity)
                    {
                        return new JsonResult("Outstock");
                    }
                    cart[index].quantity++;
                    cart[index].savePrice += savePrice;
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                    return new JsonResult("Exists "+ product_detail.ProductDetailId);
                }
            }
        }

        [Route("removeFromCart")]
        [HttpPost]
        public IActionResult removeFromCart(string product_detail_id)
        {
            // KIEM TRA KHACH HANG
            // Kiem tra khach hang thanh vien co dang nhap hay ko
            if(HttpContext.Session.GetString("customer")!=null)
            {
                var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
                _customerService.removeItemFromCartInDb(product_detail_id, customer.Id);
            }

            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            int index = Exists(product_detail_id, cart);
            cart.RemoveAt(index);

            if(cart.Count ==0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }

            CartController.promotionCode = null;

            return new JsonResult(new { message = "success" });
        }

        private int Exists(string id, List<Item> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product_detail_id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private int ExistsInCartDb(string id, string customerId)
        {
            return _customerService.checkExistsInDb(id, customerId);
        }

        private bool isViewed(string id, List<OverViewProduct> viewedProducts)
        {
            for (int i = 0; i < viewedProducts.Count; i++)
            {
                if (viewedProducts[i].CODE == id)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
