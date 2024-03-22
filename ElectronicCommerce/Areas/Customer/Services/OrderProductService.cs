using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public class OrderProductService:IOrderProductService
    {
        private DatabaseContext _db;
        private IConfiguration _configuration;
        public OrderProductService(DatabaseContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public int calculateScorePayForCustomer(int total)
        {
            return total;
        }

        public string CustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo,ElectronicCommerce.Models.Customer customer, string? code, double total)
        {
            // Cap nhat lai quantity san pham trong kho
            if (cart != null && shipInfo != null)
            {
                // Cap nhat lai quantity cua san pham

                foreach (var item in cart)
                {
                    if (item.isCheck)
                    {
                        updateProductQuantity(item);
                    }
                }
                _db.SaveChanges();

                // Tao hoa don thanh toan thanh cong

                var orderProduct = new OrderProduct();

                orderProduct.Id = "PP" + OrderCode.RandomString(6);
                orderProduct.DateCreated = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.CustomerId = customer.Id;
                orderProduct.Pay = 1;
                orderProduct.DatePay = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.PayType = "Chuyển khoản";
                orderProduct.TotalPay = (decimal?)total;
                orderProduct.OrderState = "Chờ xác nhận";
                // Neu nguoi dung thay doi thong tin giao nhan thi luu lai vao don hang
                orderProduct.AddressDelivery = shipInfo.ADDRESS;
                orderProduct.PhoneNonAccount = shipInfo.PHONE;
                orderProduct.NameCusNonAccount = shipInfo.FULLNAME;
                orderProduct.ShipDate = null;
                orderProduct.ShipFee = 0;

                // Luu customer type o thoi diem hien tai
                var cusInfo = _db.Customers.ToList().SingleOrDefault(i => i.Id.Equals(customer.Id));
                orderProduct.CustomerTypeId = cusInfo.CustomerType.Id;

                if(code !=null)
                {
                    var promotion = _db.Promotions.ToList().SingleOrDefault(i => i.Code.Equals(code));
                    orderProduct.PromotionId = promotion.Id;
                }
                // Cap nhat id cua nhan vien xac nhan don hang nay sau
                orderProduct.IdUser = null;
                orderProduct.MailNonCus = shipInfo.MAIL;

                _db.OrderProducts.Add(orderProduct);
                _db.SaveChanges();

                // Tao chi tiet hoa don thanh toan thanh cong

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.OrderDetailId = "OD" + OrderCode.RandomString(6);
                    orderDetail.OrderId = orderProduct.Id;
                    orderDetail.ProductDetailId = item.product_detail_id;
                    orderDetail.Quantity = item.quantity;
                    orderDetail.SalePrice = item.price;

                    // Cap nhat lai cac san pham da thanh toan trong bang cart
                    if (item.isCheck)
                    {
                        var cartItem = _db.Carts.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(item.product_detail_id) && i.CustomerId.Equals(customer.Id) && i.OrderId == null);
                        cartItem.OrderId = orderProduct.Id;
                        _db.Carts.Update(cartItem);
                    }

                    _db.OrderDetails.Add(orderDetail);
                }

                _db.SaveChanges();
                // Doi voi khach hang thanh vien se khong gui mail thong bao thanh cong ma thong bao o trang cua khach hang thanh vien

                // Cong scorepay cho khach hang
                var member = _db.Customers.ToList().SingleOrDefault(i => i.Id.Equals(customer.Id));

                //member.ScorePay += orderProduct.TotalPay;
                member.ScorePay += (int)orderProduct.TotalPay;

                // Kiem tra score pay dat yeu cau thi cap nhat loai khach hang

                _db.Customers.Update(member);
                _db.SaveChanges();
                return "success";
            }
            else
            {
                return "fail";
            }

        }

        public string NonCustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo, double total)
        {
            // Cap nhat lai quantity san pham trong kho
            if (cart != null && shipInfo != null)
            {
                // Cap nhat lai quantity cua san pham

                foreach (var item in cart)
                {
                    if(item.isCheck)
                    {
                        updateProductQuantity(item);
                    }
                }
                _db.SaveChanges();

                // Tao hoa don thanh toan thanh cong

                var orderProduct = new OrderProduct();

                orderProduct.Id = "PP" + OrderCode.RandomString(6);
                orderProduct.DateCreated = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // Do khong phai khach hang thanh vien nen customerid = UNKNOWN
                orderProduct.CustomerId = "UNKNOWN";
                orderProduct.AddressDelivery = shipInfo.ADDRESS;
                orderProduct.Pay = 0;
                orderProduct.DatePay = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.PayType = "Tiền mặt";
                orderProduct.TotalPay = (decimal?)total;
                orderProduct.OrderState = "Chờ xác nhận";
                orderProduct.PhoneNonAccount = shipInfo.PHONE;
                orderProduct.NameCusNonAccount = shipInfo.FULLNAME;
                orderProduct.ShipDate = null;
                orderProduct.ShipFee = 0;
                // Cap nhat id cua nhan vien xac nhan don hang nay sau
                orderProduct.IdUser = null;
                orderProduct.MailNonCus = shipInfo.MAIL;

                _db.OrderProducts.Add(orderProduct);
                _db.SaveChanges();

                // Tao chi tiet hoa don thanh toan thanh cong

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.OrderDetailId = "OD" + OrderCode.RandomString(6);
                    orderDetail.OrderId = orderProduct.Id;
                    orderDetail.ProductDetailId = item.product_detail_id;
                    orderDetail.Quantity = item.quantity;
                    orderDetail.SalePrice = item.price;

                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // Gui mail thong bao thanh toan thanh cong cho khach hang cung voi ma don hang
                var mailHelper = new MailHelper(_configuration);
                string to = shipInfo.MAIL;
                string subject = "Thanh toán thành công tại HNJ";
                string content = "<table style='margin-bottom:20px;' width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td align='center' style='text-align: center;'><h1 style='color:#DCB15B;font-size: 40px;margin: 0;'>Thanh toán thành công</h1> <h3>IJC cám ơn bạn đã tin tưởng chọn và chọn chúng tôi</h3> <p>Bạn đã thực hiện thanh toán thành công qua PayPal</p> <p style='font-weight: bolder;'>Mã đơn hàng của bạn là:</p> <span style='color:#DCB15B; border:1px solid #E5E5E5; font-size: 40px; padding: 10px;'>" + orderProduct.Id + "</span> </td> </tr> </table>";
                string from = "nghiamc147@gmail.com";

                mailHelper.Send(from, to, subject, content);

                return "success";
            }
            else
            {
                return "fail";
            }
        }

        public string NonCustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo, double total)
        {
            // Cap nhat lai quantity san pham trong kho
            if(cart !=null && shipInfo !=null)
            {
                // Cap nhat lai quantity cua san pham

                foreach (var item in cart)
                {
                    if (item.isCheck)
                    {
                        updateProductQuantity(item);
                    }
                }
                _db.SaveChanges();

                // Tao hoa don thanh toan thanh cong

                var orderProduct = new OrderProduct();

                orderProduct.Id = "PP" + OrderCode.RandomString(6);
                orderProduct.DateCreated = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // Do khong phai khach hang thanh vien nen customerid = UNKNOWN
                orderProduct.CustomerId = "UNKNOWN";
                orderProduct.AddressDelivery = shipInfo.ADDRESS;
                orderProduct.Pay = 1;
                orderProduct.DatePay = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"),"dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.PayType = "Chuyển khoản";
                orderProduct.TotalPay = (decimal?)total;
                orderProduct.OrderState = "Chờ xác nhận";
                orderProduct.PhoneNonAccount = shipInfo.PHONE;
                orderProduct.NameCusNonAccount = shipInfo.FULLNAME;
                orderProduct.ShipDate = null;
                orderProduct.ShipFee = 0;
                // Cap nhat id cua nhan vien xac nhan don hang nay sau
                orderProduct.IdUser = null;
                orderProduct.MailNonCus = shipInfo.MAIL;

                _db.OrderProducts.Add(orderProduct);
                _db.SaveChanges();

                // Tao chi tiet hoa don thanh toan thanh cong

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.OrderDetailId = "OD" + OrderCode.RandomString(6);
                    orderDetail.OrderId = orderProduct.Id;
                    orderDetail.ProductDetailId = item.product_detail_id;
                    orderDetail.Quantity = item.quantity;
                    orderDetail.SalePrice = item.price;

                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // Gui mail thong bao thanh toan thanh cong cho khach hang cung voi ma don hang
                var mailHelper = new MailHelper(_configuration);
                string to = shipInfo.MAIL;
                string subject = "Thanh toán thành công tại HNJ";
                string content = "<table style='margin-bottom:20px;' width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td align='center' style='text-align: center;'> <img src='https://ijc.vn/vnt_upload/weblink/Logo_IJC__Slogan_1.png' width='220' height='100'> <h1 style='color:#DCB15B;font-size: 40px;margin: 0;'>Thanh toán thành công</h1> <h3>IJC cám ơn bạn đã tin tưởng chọn và chọn chúng tôi</h3> <p>Bạn đã thực hiện thanh toán thành công qua PayPal</p> <p style='font-weight: bolder;'>Mã đơn hàng của bạn là:</p> <span style='color:#DCB15B; border:1px solid #E5E5E5; font-size: 40px; padding: 10px;'>"+ orderProduct.Id + "</span> </td> </tr> </table>";
                string from = "tung.lt712@gmail.com";

                mailHelper.Send(from, to, subject, content);

                return "success";
            }
            else
            {
                return "fail";
            }

        }

        private bool updateProductQuantity(Item item)
        {
            if(item != null)
            {
                var productDetail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(item.product_detail_id));
                productDetail.Quantity -= item.quantity;
                _db.ProductDetails.Update(productDetail);
                return true;
            }
            return false;
        }

        public void UpdateQuantityCancelOrder(OrderProduct order)
        {
            var productInOrder = order.OrderDetails.Count;
            if(productInOrder >1)
            {
                foreach (var item in order.OrderDetails.ToList())
                {
                    var productDetail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(item.ProductDetailId));
                    productDetail.Quantity += item.Quantity;
                    _db.ProductDetails.Update(productDetail);
                }
            }
            else
            {
                var productDetail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(order.OrderDetails.ToList()[0].ProductDetailId));
                productDetail.Quantity += order.OrderDetails.ToList()[0].Quantity;
                _db.ProductDetails.Update(productDetail);
            }

            _db.SaveChanges();
        }

        public string CustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo, ElectronicCommerce.Models.Customer customer, string code, double total)
        {
            // Cap nhat lai quantity san pham trong kho
            if (cart != null && shipInfo != null)
            {
                // Cap nhat lai quantity cua san pham

                foreach (var item in cart)
                {
                    if (item.isCheck)
                    {
                        updateProductQuantity(item);
                    }
                }
                _db.SaveChanges();

                // Tao hoa don thanh toan thanh cong

                var orderProduct = new OrderProduct();

                orderProduct.Id = "PP" + OrderCode.RandomString(6);
                orderProduct.DateCreated = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.CustomerId = customer.Id;
                orderProduct.Pay = 1;
                orderProduct.DatePay = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderProduct.PayType = "Tiền mặt";
                orderProduct.TotalPay = (decimal?)total;
                orderProduct.OrderState = "Chờ xác nhận";
                // Neu nguoi dung thay doi thong tin giao nhan thi luu lai vao don hang
                orderProduct.AddressDelivery = shipInfo.ADDRESS;
                orderProduct.PhoneNonAccount = shipInfo.PHONE;
                orderProduct.NameCusNonAccount = shipInfo.FULLNAME;
                orderProduct.ShipDate = null;
                orderProduct.ShipFee = 0;
                // Luu customer type o thoi diem hien tai
                var cusInfo = _db.Customers.ToList().SingleOrDefault(i => i.Id.Equals(customer.Id));
                orderProduct.CustomerTypeId = cusInfo.CustomerType.Id;

                if (code != null)
                {
                    var promotion = _db.Promotions.ToList().SingleOrDefault(i => i.Code.Equals(code));
                    orderProduct.PromotionId = promotion.Id;
                }
                // Cap nhat id cua nhan vien xac nhan don hang nay sau
                orderProduct.IdUser = null;
                orderProduct.MailNonCus = shipInfo.MAIL;

                _db.OrderProducts.Add(orderProduct);
                _db.SaveChanges();

                // Tao chi tiet hoa don thanh toan thanh cong

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.OrderDetailId = "OD" + OrderCode.RandomString(6);
                    orderDetail.OrderId = orderProduct.Id;
                    orderDetail.ProductDetailId = item.product_detail_id;
                    orderDetail.Quantity = item.quantity;
                    orderDetail.SalePrice = item.price;

                    // Cap nhat lai cac san pham da thanh toan trong bang cart
                    if (item.isCheck)
                    {
                        var cartItem = _db.Carts.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(item.product_detail_id) && i.CustomerId.Equals(customer.Id) && i.OrderId == null);
                        cartItem.OrderId = orderProduct.Id;
                        _db.Carts.Update(cartItem);
                    }

                    _db.OrderDetails.Add(orderDetail);
                }

                _db.SaveChanges();
                // Doi voi khach hang thanh vien se khong gui mail thong bao thanh cong ma thong bao o trang cua khach hang thanh vien

                return "success";
            }
            else
            {
                return "fail";
            }

        }
    }
}
