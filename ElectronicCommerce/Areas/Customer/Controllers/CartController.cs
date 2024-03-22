using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using DemoSession14.Paypal;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/cart")]
    public class CartController : Controller
    {
        private IBaseRepository<ProductDetail> _productDetailRepo;
        private IConfiguration configuration;
        private IOrderProductService _orderProductService;
        private ICustomerService _customerService;
        private DatabaseContext _db;
        public static string promotionCode;

        public CartController(IBaseRepository<ProductDetail> productDetailRepo, IConfiguration _configuration, IOrderProductService orderProductService,
            ICustomerService customerService, DatabaseContext db)
        {
            _productDetailRepo = productDetailRepo;
            configuration = _configuration;
            _orderProductService = orderProductService;
            _customerService = customerService;
            _db = db;
        }
        // GET: /<controller>/
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("")]
        [Route("CartPage")]
        public IActionResult CartPage()
        {

            if (HttpContext.Session.GetString("cart") == null)
            {
                return View("cartpage");
            }
            else
            {
                var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.cartTotal = cartSession.ToList().Sum(i => i.price * i.quantity);
                ViewBag.cart = cartSession;
                return View("cartpage");
            }
        }

        [Route("BookingPage")]
        public IActionResult BookingPage()
        {
            ViewBag.PostUrl = configuration["PayPal:PostUrl"].ToString();
            ViewBag.ReturnUrl = configuration["PayPal:ReturnUrl"].ToString();
            ViewBag.Business = configuration["PayPal:Business"].ToString();

            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

            ViewBag.products = cart.ToList().Where(i => i.isCheck).ToList();

            double total = cart.ToList().Where(i => i.isCheck).Sum(p => p.price * p.quantity);

            if (HttpContext.Session.GetString("customer")!=null)
            {
                var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

                ViewBag.customerId = customer.Id;

                // Giam gia dua tren member type

                var customerInfo = _customerService.findCustomerById(customer.Id);
                //total = total * ((double)(100 - customerInfo.CustomerType.DiscountValue)/ (double)100);
                total = total;
                // Load promotion cua khach hang thanh vien

                var result = _customerService.findAllPromotionsOfCustomer(customer.Id);
                if(result !=null && result.Count >0)
                {
                    ViewBag.promos = result;
                }

                
            }

            ViewBag.pointCount = (int)total;

            ViewBag.total = total;
            return View("bookingpage");
        }

        [HttpPost]
        [Route("updateQuantity")]
        public IActionResult UpdateQuantity(string product_detail_id, string action)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            for (int i = 0; i < cart.Count; i++)
            {
                if(cart[i].product_detail_id.Equals(product_detail_id))
                {
                    if (HttpContext.Session.GetString("customer") != null)
                    {
                        var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
                        int indexCartDB = ExistsInCartDb(product_detail_id, customer.Id);

                        if (indexCartDB != -1)
                        {
                            _customerService.updateQuantityCartInDb(indexCartDB, action);
                        }
                    }
                    if (action.Equals("minus"))
                    {
                        cart[i].quantity -= 1;
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                        return RedirectToAction("CartPage");

                    }
                    else if(action.Equals("plus"))
                    {
                        if(cart[i].quantity == _productDetailRepo.GetById(product_detail_id).Quantity)
                        {
                            TempData["outstock"] = "Vượt quá số lượng ở kho";
                            return RedirectToAction("CartPage");
                        }
                        cart[i].quantity += 1;
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                        return RedirectToAction("CartPage");
                    }
                }
            }
            return RedirectToAction("CartPage");
        }

        [HttpGet]
        [Route("isShipInformation")]
        public IActionResult isShipInformation()
        {
            if (HttpContext.Session.GetString("shipping_info") != null)
            {
                var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
                return new JsonResult(shipInfo);
            }
            else
            {
                return new JsonResult(null);
            }
        }

        [HttpPost]
        [Route("saveShipInformation")]
        public IActionResult saveShipInformation(string fullname, string mail, string phone, string address)
        {
            // Kiem tra khach hang thanh vien co dang nhap
            // Neu co luu thong tin giao nhan hang vao db
            if (HttpContext.Session.GetString("customer") != null)
            {
                var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

                var shippingInfo = new ShippingInformation();

                shippingInfo.FULLNAME = fullname;
                shippingInfo.MAIL = mail;
                shippingInfo.PHONE = phone;
                shippingInfo.ADDRESS = address;

                _customerService.updateCustomerInfo(shippingInfo, customer.Id);

            }


            // Tao session luu thong tin giao hang
            if (HttpContext.Session.GetString("shipping_info") == null)
            {
                var shippingInfo = new ShippingInformation();

                shippingInfo.FULLNAME = fullname;
                shippingInfo.MAIL = mail;
                shippingInfo.PHONE = phone;
                shippingInfo.ADDRESS = address;

                HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shippingInfo));
                return new JsonResult(new { message = "create session shipping success" });
            } else
            {
                var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));

                shipInfo.FULLNAME = fullname;
                shipInfo.MAIL = mail;
                shipInfo.PHONE = phone;
                shipInfo.ADDRESS = address;

                HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shipInfo));
                return new JsonResult(new { message = "save session shipping success" });
            }
        }


        [HttpPost]
        [Route("updatePaidProduct")]
        public IActionResult UpdatePaidProduct(string product_detail_id, bool isCheck)
        {
            // Kiem tra cap nhat isCheck trong cartDb cua khach hang
            if(HttpContext.Session.GetString("customer") !=null)
            {
                var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
                _customerService.updateIsCheckCartInDb(product_detail_id, isCheck, customer.Id);
            }


            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product_detail_id.Equals(product_detail_id))
                {
                    cart[i].isCheck = isCheck;                    
                }
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return new JsonResult(new { message = "Update paid product success" });
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success(double amt, string cc, string st, string tx)
        {
            // TAO HOA DON LUU VAO DB
            var result = PDTHolder.Success(tx, configuration, Request);
            Debug.WriteLine(result);

            var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));


            if (result.PaymentStatus.Equals("Completed"))
            {
                // Kiem tra khach hang thanh vien hay khach vang lai thuc hien thanh toan
                // Neu la khach vang lai
                if(HttpContext.Session.GetString("customer") == null)
                {
                    // Goi dich vu thanh toan thanh cong de tao hoa don hoan thanh don hang cho khach vang lai
                    _orderProductService.NonCustomerPayPalSuccess(cart, shipInfo,result.GrossTotal);

                    //return View("success");
                    return RedirectToAction("noncustomerpaysuccess", new { message = tx });
                }
                // Neu la khach hang thanh vien
                else
                {
                    var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
                    _orderProductService.CustomerPayPalSuccess(cart, shipInfo, customer,promotionCode, result.GrossTotal);
                    return RedirectToAction("customerpaysuccess", new { message = tx });
                }

            }
            else
            {
                return View("fail");
            }
        }

        // Theo quy dinh ve score pay va loai khach hang
        //private string UpdateCustomerTypeByScorePay(string customer_id)
        //{

        //}

        [HttpGet]
        [Route("customerpaysuccess")]
        public IActionResult CustomerPaySuccess(string message)
        {
            // TAO HOA DON LUU VAO DB
            var result = PDTHolder.Success(message, configuration, Request);

            var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

            if (result.PaymentStatus.Equals("Completed"))
            {
                ViewBag.transactionId = result.TransactionId;
                ViewBag.mailReciever = result.ReceiverEmail;
                ViewBag.shipTo = shipInfo.ADDRESS;
                ViewBag.carts = cart.ToList().Where(i => i.isCheck).ToList();
                ViewBag.totalPay = result.GrossTotal;

                // Xoa nhung san pham da thanh toan trong cart
                cart = cart.ToList().Where(i => i.isCheck == false).ToList();

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));


                return View("paypalsuccess");
            }
            else
            {
                return View("fail");
            }
        }

        [HttpPost]
        [Route("ApplyPromotion")]
        public IActionResult ApplyPromotion(string promoCode, double cartTotal)
        {
            // Doc gia tri tu session customer_test
            int total = (int) cartTotal;
            var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

            var result = _customerService.checkValidPromo(customer.Id, promoCode, total);
            if (result.Equals("valid"))
            {
                promotionCode = promoCode;
                var promoOfCustomer = _customerService.findAllPromotionsOfCustomer(customer.Id).SingleOrDefault(i => i.Code.Equals(promoCode));

                var promotion = new Promotion();
                promotion.Id = promoOfCustomer.Id;
                promotion.Code = promoOfCustomer.Code;
                promotion.DiscountUnit = promoOfCustomer.DiscountUnit;
                promotion.DiscountValue = promoOfCustomer.DiscountValue;
                promotion.MaxDiscount = promoOfCustomer.MaxDiscount;
                promotion.MinOrder = promoOfCustomer.MaxDiscount;

                return new JsonResult(promotion);
            }
            else
            {
                return new JsonResult(result);
            }

        }

        [HttpGet]
        [Route("noncustomerpaysuccess")]
        public IActionResult NonCustomerPaySuccess(string message)
        {
            // TAO HOA DON LUU VAO DB
            var result = PDTHolder.Success(message, configuration, Request);

            var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

            if (result.PaymentStatus.Equals("Completed"))
            {
                ViewBag.transactionId = result.TransactionId;
                ViewBag.mailReciever = result.ReceiverEmail;
                ViewBag.shipTo = shipInfo.ADDRESS;
                ViewBag.carts = cart.ToList().Where(i => i.isCheck).ToList();
                ViewBag.totalPay = result.GrossTotal;

                // Xoa nhung san pham da thanh toan trong cart
                cart = cart.ToList().Where(i => i.isCheck == false).ToList();

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));


                return View("paypalsuccess");
            }
            else
            {
                return View("fail");
            }
        }
        private int ExistsInCartDb(string id, string customerId)
        {
            return _customerService.checkExistsInDb(id, customerId);
        }

        [HttpGet]
        [Route("noncustomercodsuccess")]
        public IActionResult NonCustomerCodSuccess(double total)
        {
            Debug.WriteLine(total);
            var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
            var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

            var sInfo = new ShippingInformation();

            sInfo.FULLNAME = shipInfo.FULLNAME;
            sInfo.MAIL = shipInfo.MAIL;
            sInfo.ADDRESS = shipInfo.ADDRESS;
            sInfo.PHONE = shipInfo.PHONE;

            ViewBag.ship = sInfo;

            // Luu don hang vao db
            
            _orderProductService.NonCustomerCodSuccess(cartSession, shipInfo,total);

            // Xoa nhung san pham da thanh toan trong cart
            cartSession = cartSession.ToList().Where(i => i.isCheck == false).ToList();

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartSession));

            return View("codsuccess");
        }

        [HttpGet]
        [Route("customercodsuccess")]
        public IActionResult CustomerCodSuccess(double total)
        {
            Debug.WriteLine(total);
            // TAO HOA DON LUU VAO DB
            var shipInfo = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

            var sInfo = new ShippingInformation();

            sInfo.FULLNAME = shipInfo.FULLNAME;
            sInfo.MAIL = shipInfo.MAIL;
            sInfo.ADDRESS = shipInfo.ADDRESS;
            sInfo.PHONE = shipInfo.PHONE;

            ViewBag.ship = sInfo;

            // Luu don hang vao db
            var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
            _orderProductService.CustomerCodSuccess(cart, shipInfo,customer,promotionCode, total);

            // Xoa nhung san pham da thanh toan trong cart
            cart = cart.ToList().Where(i => i.isCheck == false).ToList();

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            return View("codsuccess");
        }

        [HttpPost]
        [Route("cod")]
        public IActionResult Cod(string total)
        {
            double totalPay = double.Parse(total);
            if (HttpContext.Session.GetString("customer") !=null)
            {
                return RedirectToAction("customercodsuccess", new {total = totalPay });
            }
            else
            {
                return RedirectToAction("noncustomercodsuccess", new { total = totalPay });
            }
            
        }

    }
}
