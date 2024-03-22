using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using ElectronicCommerce.Areas.Customer.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ElectronicCommerce.Areas.Customer.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Admin.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/customer")]
    public class CustomerController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<ElectronicCommerce.Models.Customer> _baseRepoCustomer;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private ICustomerService _customerService;
        private INotyfService _notyfService;
        private IConfiguration _configuration;
        private IBaseRepository<OrderProduct> _baseOrderProduct;
        private IBaseRepository<Review> _baseReview;
        private IOrderProductService _orderProductService;

        private IWebHostEnvironment _webHostEnvironment; // !IMPORTANT

        public CustomerController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , ICustomerService customerService, IBaseRepository<ElectronicCommerce.Models.Customer> baseRepoCustomer
            ,INotyfService notyfService,IConfiguration configuration, IWebHostEnvironment webHostEnvironment
            , IBaseRepository<OrderProduct> baseOrderProduct, IBaseRepository<Review> baseReview
            ,IOrderProductService orderProductService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _customerService = customerService;
            _baseRepoCustomer = baseRepoCustomer;
            _notyfService = notyfService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _baseOrderProduct = baseOrderProduct;
            _baseReview = baseReview;
            _orderProductService = orderProductService;
        }
        // GET: /<controller>/
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

            ViewBag.customer = customer;

            return View();
        }

        

        [Route("order")]
        public IActionResult Order()
        {
            var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
            var orderOfCustomer = _customerService.findAllOrderProductOfCustomer(customer.Id);
            if(orderOfCustomer.Count > 0 && orderOfCustomer !=null )
            {
                ViewBag.orders = orderOfCustomer;
            }
            return View("order");
        }

        // Cancel order
        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel(string id)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            var order = _baseOrderProduct.GetAll().ToList().SingleOrDefault(p=> p.Id.Equals(id));

            if(order.OrderState.Equals("Chờ xác nhận"))
            {
                // Huy don hang
                CancelOrder(order);
                // Cap nhat lai quantity cho cac san pham trong don hang


                // Thong bao huy don thanh cong
                return new JsonResult(new { message = "ok" });
            }
            else if(order.OrderState.Equals("Đã xác nhận"))
            {
                // Gui yeu cau huy don
                RequestCancelOrder(order);
                return new JsonResult(new { message = "sent" });

            }
            else if (order.OrderState.Equals("Đang giao hàng"))
            {
                // Thong bao khong the huy don
                return new JsonResult(new { message = "unable" });
            }
            else
            {
                return new JsonResult(new { message = "error" });
            }
        }

        // Method cancel order
        private void CancelOrder(OrderProduct order)
        {
            order.OrderState = "Đã huỷ đơn";
            _baseOrderProduct.Update(order);
            _baseOrderProduct.Save();
            _orderProductService.UpdateQuantityCancelOrder(order);
        }

        // Method request cancel order
        private void RequestCancelOrder(OrderProduct order)
        {
            order.OrderState = "Đã yêu cầu huỷ đơn";
            _baseOrderProduct.Update(order);
            _baseOrderProduct.Save();
        }


        // Login page
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();
            return View("Login");
        }

        [HttpGet]
        [Route("Info")]
        public IActionResult Info()
        {
            var member = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

            var customer = _customerService.findCustomerById(member.Id);

            ViewBag.customer = customer;

            return View("Info", customer);
        }

        [HttpGet]
        [Route("Review/{id}")]
        public IActionResult Review(string id)
        {
            var order = _baseOrderProduct.GetById(id);

            ViewBag.order = order;

            return View("AddReview", new Review());
        }

        [HttpPost]
        [Route("AddReview")]
        public IActionResult AddReview(Review review, string order_id)
        {
            var order = _baseOrderProduct.GetById(order_id);

            ViewBag.order = order;

            var cus = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
            if (review !=null)
            {
                review.Id = "RV" + OrderCode.RandomString(6);
                review.Created_Date = DateTime.Today;
                review.Is_Update = false;
                review.CustomerId = cus.Id;
                _baseReview.Insert(review);
                _baseReview.Save();
                TempData["msg"] = "Gửi đánh giá thành công";
                return View("AddReview", new Review());
            }
            else
            {
                TempData["msg"] = "Thêm đánh giá không thành công";
                return View("AddReview", new Review());
            }
        }

        [HttpPost]
        [Route("UpdateCustomerInfo")]
        public IActionResult UpdateCustomerInfo(ElectronicCommerce.Models.Customer customer, IFormFile photo)
        {
            if (photo != null)
            {
                var newFileName = Admin.Helpers.FileHelper.GenerateFileName(photo.ContentType);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "customer/img", newFileName);
                using (var fileStream = new FileStream(path, FileMode.Create)) // !IMPORTANT
                {
                    // upload file vao fileStream
                    photo.CopyTo(fileStream);
                }

                customer.Avatar = newFileName;

                // Cap nhat lai session customer
                var cus = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));

                cus.Avatar = newFileName;

                HttpContext.Session.SetString("customer", ElectronicCommerce.Models.Customer.ToJson(cus));


            }

            _baseRepoCustomer.Update(customer);
            _baseRepoCustomer.Save();

            // Cap nhat lai shipping info
            if (_customerService.isCustomerFilledInfo(customer.Id))
            {
                var shippingInfo = new ShippingInformation();

                shippingInfo.FULLNAME = customer.Fullname;
                shippingInfo.MAIL = customer.Username;
                shippingInfo.PHONE = customer.Phone;
                shippingInfo.ADDRESS = customer.Address;

                HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shippingInfo));
            }


            return RedirectToAction("info");
        }

        [Route("GoogleLogin")]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("googleresponse")
            });
        }

        [HttpPost]
        [Route("findAllPromotionsByCustomerId")]
        public IActionResult findAllPromotionsByCustomerId(string customer_id)
        {
            var result = _customerService.findAllPromotionsOfCustomer(customer_id);
            return new JsonResult(result);
        }

        [Route("googleresponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                        .FirstOrDefault().Claims.Select(claim => new
                        {
                            claim.Issuer,
                            claim.OriginalIssuer,
                            claim.Type,
                            claim.Value
                        });
            //Mail cua khach hang thanh vien
            string customerGmail = claims.ElementAt(4).Value;
            Debug.WriteLine("Gmail cua customer: "+ customerGmail);

            var customer = _customerService.isExistMail(customerGmail);

            if(customer !=null && customer.Password !=null)
            {
                ViewBag.cates = _baseRepoCate.GetAll().ToList();
                ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

                _notyfService.Information("Địa chỉ mail này đã được đăng ký thành viên bằng tài khoản và mật khẩu", 3);
                return View("login");
            }
            else
            {
                // Goi dich vu khach hang dang nhap bang google
                var cus = _customerService.signInWithGoogle(customerGmail);
                return RedirectToAction("loginsuccess", new { message = "success", customerId = cus.Id });
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(string mail)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();
            if (mail != null && mail.Length > 0)
            {
                // Kiem tra mail nay da dang ky thanh vien truoc do chua
                var isCustomerBefore = _customerService.isExistMail(mail);

                if (isCustomerBefore != null)
                {
                    _notyfService.Information("Địa chỉ mail này đã được đăng ký thành viên trước đó. Vui lòng chọn đăng nhập với gmail , facebook hoặc chọn quên mật khẩu.", 3);
                    return View("login");
                }
                else
                {
                    var password = Admin.Helpers.OrderCode.RandomString(6);

                    var mailHelper = new MailHelper(_configuration);
                    string to = mail;
                    string subject = "Đăng ký thành viên thành công tại HNJ";
                    string content = "<table style='margin-bottom:20px;' width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td align='center' style='text-align: center;'> " +
                        " <h1 style='color:#DCB15B;font-size: 40px;margin: 0;'>Đăng ký thành công</h1> <h3>HNJ cám ơn bạn đã tin tưởng chọn và chọn chúng tôi</h3> <p>Bạn đã đăng ký thành công, vui lòng thay đổi mật khẩu sau khi đăng nhập</p> <p style='font-weight: bolder;'>Mật khẩu của bạn là, vui lòng không chia sẽ mật khẩu này:</p> <span style='color:#DCB15B; border:1px solid #E5E5E5; font-size: 40px; padding: 10px;'>" + password + "</span> </td> </tr> </table>";
                    string from = "tung.lt712@gmail.com";

                    if (mailHelper.Send(from, to, subject, content))
                    {
                        var customer = new ElectronicCommerce.Models.Customer();

                        customer.Id = "MB" + OrderCode.RandomString(6);
                        customer.Username = mail;
                        customer.Avatar = "temp-avt.jpg";
                        customer.Password = BCrypt.Net.BCrypt.HashPassword(password);

                        _baseRepoCustomer.Insert(customer);
                        _baseRepoCustomer.Save();

                        _notyfService.Success("Đăng ký thành công. Mật khẩu đã được gửi đến mail của bạn. Vui lòng thay đổi mật khẩu sau khi đăng nhập.", 3);
                    }
                    else
                    {
                        _notyfService.Error("Không tìm thấy địa chỉ mail của bạn, vui lòng kiểm tra lại", 3);
                    }
                    return View("login");
                }
            }
            else
            {
                _notyfService.Error("Vui lòng nhập vào gmail của bạn", 3);
                return View("login");
            }
        }

        [HttpPost]
        [Route("accountlogin")]
        public IActionResult AccountLogin(string username, string password)
        {
            var customer = _customerService.isExistMail(username);
            if (customer !=null)
            {
                if(customer.Password == null)
                {
                    ViewBag.cates = _baseRepoCate.GetAll().ToList();
                    ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

                    _notyfService.Information("Tài khoản này được đăng ký thành viên qua hình thức xác thực bằng GOOGLE hoặc FACEBOOK", 3);

                    return View("login");
                }
                else
                {
                    if(BCrypt.Net.BCrypt.Verify(password,customer.Password))
                    {
                        return RedirectToAction("loginsuccess", new { message = "success", customerId = customer.Id });
                    }
                    else
                    {
                        ViewBag.cates = _baseRepoCate.GetAll().ToList();
                        ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

                        _notyfService.Error("Vui lòng kiểm tra lại mật khẩu của bạn", 3);
                        return View("login");
                    }
                }
            }
            else
            {
                ViewBag.cates = _baseRepoCate.GetAll().ToList();
                ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

                _notyfService.Error("Địa chỉ mail của bạn chưa đăng ký thành viên", 3);
                return View("login");
            }
        }


        [HttpGet]
        [Route("loginsuccess")]
        public IActionResult Login(string? message,string? customerId)
        {
            var customer = _customerService.findCustomerById(customerId);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (message.Length >0 || message !=null)
            {
                if(message.Equals("success") && customer !=null)
                {
                    // Login thanh cong tao session

                    var signedCustomer = new ElectronicCommerce.Models.Customer();
                    signedCustomer.Id = customer.Id;
                    signedCustomer.Username = customer.Username;
                    signedCustomer.Avatar = customer.Avatar;
                    if(customer.Address !=null)
                    {
                        signedCustomer.Address = customer.Address;
                    }

                    if(signedCustomer.Phone !=null)
                    {
                        signedCustomer.Phone = customer.Phone;
                    }


                    HttpContext.Session.SetString("customer", ElectronicCommerce.Models.Customer.ToJson(signedCustomer));

                    // KHACH HANG CHECK
                    // Cap nhat gio hang cho khach hang neu co
                    // Neu truoc do ton tai session gio hang thi sau khi khach hang dang nhap
                    // Thuc hien cap nhat gio hang vao cart trong db
                    if(HttpContext.Session.GetString("cart") !=null)
                    {
                        var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

                        // Goi dich vu cap nhat session va gio hang
                        _customerService.updateCartInDb(cartSession, customer);

                        // Do dbcart cua customer vao session cart
                        cartSession = _customerService.findAllCartByCustomerId(customer.Id);
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartSession, Formatting.Indented, settings));
                    }
                    else
                    {
                        // Do dbcart cua customer vao session cart
                        var cartSession = _customerService.findAllCartByCustomerId(customer.Id);
                        if(cartSession.Count >0)
                        {
                            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartSession, Formatting.Indented, settings));
                        }
                    }

                    // Kiem tra shipping_info session co duoc tao chua
                    // Neu co
                    if (HttpContext.Session.GetString("shipping_info") != null)
                    {
                        // Neu khach hang chua co thong tin
                        // Ma truoc do da co shipping_info
                        var shipping_info = JsonConvert.DeserializeObject<ShippingInformation>(HttpContext.Session.GetString("shipping_info"));
                        if (!(_customerService.isCustomerFilledInfo(customer.Id)))
                        {
                            // Cap nhat value tu shipping_info vao cho thong tin cua khach hang
                            // Neu nhu mail tu session shipping_info khop voi mail khach hang dang nhap thi cap nhat khong thi huy session
                            if(customer.Username.Equals(shipping_info.MAIL))
                            {
                                customer.Address = shipping_info.ADDRESS;
                                customer.Fullname = shipping_info.FULLNAME;
                                customer.Phone = shipping_info.PHONE;
                            }
                            else
                            {
                                HttpContext.Session.Remove("shipping_info");
                            }
                        }
                        // Neu khach hang co day du thong tin thi cap nhat nguoc lai
                        else
                        {
                            shipping_info.MAIL = customer.Username;
                            shipping_info.ADDRESS = customer.Address;
                            shipping_info.FULLNAME = customer.Fullname;
                            shipping_info.PHONE = customer.Phone;
                            HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shipping_info));
                        }
                    }
                    // Neu chua
                    else
                    {
                        // Kiem tra xem khach hang co thong tin giao nhan hay khong
                        // Neu co thi tao session shipping_info do du lieu tu db len
                        if (_customerService.isCustomerFilledInfo(customerId))
                        {
                            var shippingInfo = new ShippingInformation();

                            shippingInfo.FULLNAME = customer.Fullname;
                            shippingInfo.MAIL = customer.Username;
                            shippingInfo.PHONE = customer.Phone;
                            shippingInfo.ADDRESS = customer.Address;

                            HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shippingInfo));
                        }
                    }

                    return RedirectToAction("index", "home");
                }
                else
                {
                    return View("login");
                }
            }
            else
            {
                return View("pagenotfound");
            }
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // Xoa session customer
            if (HttpContext.Session.GetString("customer") !=null)            {
                HttpContext.Session.Remove("customer");
            }

            // Xoa session shipinfo
            if (HttpContext.Session.GetString("shipping_info") != null)
            {
                HttpContext.Session.Remove("shipping_info");
            }

            // Xoa session cart

            if (HttpContext.Session.GetString("cart") != null)
            {
                HttpContext.Session.Remove("cart");
            }

            await HttpContext.SignOutAsync();
            return RedirectToAction("index","home");
        }

        [HttpGet]
        [Route("updatepassword")]
        public IActionResult UpdatePassword()
        {
            var cus = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
            ViewBag.customer = cus;

            return View("update_password", cus);
        }

        [HttpPost]
        [Route("updatepassword")]
        public IActionResult UpdatePassword(ElectronicCommerce.Models.Customer customer, string old_password, string new_password, string verify_password)
        {
            var cus = _customerService.findCustomerById(customer.Id);
            ViewBag.customer = cus;
            // So khop mat khau cu hay khong
            // Neu khop
            if (BCrypt.Net.BCrypt.Verify(old_password,cus.Password))
            {
                // So khop new_password va verify_password
                // Neu khop
                if (new_password.Equals(verify_password))
                {
                    // Cap nhat mat khau moi
                    cus.Password = BCrypt.Net.BCrypt.HashPassword(new_password);
                    _baseRepoCustomer.Update(cus);
                    _baseRepoCustomer.Save();

                    TempData["msg"] = "Thay đổi mật khẩu thành công";

                    return View("update_password",cus);
                }
                // Neu khong
                else
                {
                    TempData["msg"] = "Mật khẩu xác thực không khớp với mật khẩu mới";
                    return View("update_password",cus);
                }
            }
            // Neu khong
            else
            {
                TempData["msg"] = "Mật khẩu cũ không chính xác";
                return View("update_password",cus);
            }
        }



        [HttpPost]
        [Route("forgot")]
        public IActionResult Forgot(string mail)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            if (mail !=null && mail.Length >0)
            {
                if(_customerService.isExistMail(mail)!=null)
                {
                    // Gui mat khau da duoc reset va tao moi qua mail

                    var password = OrderCode.RandomString(6);

                    var mailHelper = new MailHelper(_configuration);
                    string to = mail;
                    string subject = "Xác thực tài khoản tại HNJ";
                    string content = "<table style='margin-bottom:20px;' width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td align='center' style='text-align: center;'> <img src='https://ijc.vn/vnt_upload/weblink/Logo_IJC__Slogan_1.png' width='220' height='100'> <h1 style='color:#DCB15B;font-size: 40px;margin: 0;'>Yêu cầu xác thực thành công</h1> <h3>IJC cám ơn bạn đã tin tưởng chọn và chọn chúng tôi</h3> <p>Bạn đã đặt lại mật khẩu thành công, mật khẩu hệ thống đã đặt lại cho bạn là:</p> <p style='font-weight: bolder;'>Mật khẩu của bạn là, vui lòng không chia sẽ mật khẩu này:</p> <span style='color:#DCB15B; border:1px solid #E5E5E5; font-size: 40px; padding: 10px;'>" + password + "</span> </td> </tr> </table>";
                    string from = "tung.lt712@gmail.com";

                    if (mailHelper.Send(from, to, subject, content))
                    {
                        // Gui thanh cong luu lai mat khau moi cho customer
                        var customer = _customerService.findCustomerByMail(mail);

                        customer.Password = BCrypt.Net.BCrypt.HashPassword(password);

                        _baseRepoCustomer.Update(customer);
                        _baseRepoCustomer.Save();

                        _notyfService.Success("Yêu cầu xác thực thành công. Mật khẩu đã được gửi đến mail của bạn. Vui lòng thay đổi mật khẩu sau khi đăng nhập.", 3);
                    }
                    else
                    {
                        _notyfService.Error("Không tìm thấy địa chỉ mail của bạn, vui lòng kiểm tra lại", 3);
                    }
                }
                else
                {
                    _notyfService.Error("Không tìm thấy địa chỉ mail của bạn, vui lòng kiểm tra lại", 3);
                }
                return View("login");

            }
            else
            {
                _notyfService.Error("Vui lòng nhập vào gmail của bạn", 3);
                return View("login");
            }
        }
    }
}
