using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;
using ElectronicCommerce.Areas.Admin.Helpers;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public class CustomerService:ICustomerService
    {
        private DatabaseContext _db;
        public CustomerService(DatabaseContext db)
        {
            _db = db;
        }

        public void addToCartInDb(int savePrice, Boolean isCheck,string product_detail_id, ElectronicCommerce.Models.Customer customer, int quantity)
        {
            var cartItem = new Cart();
            cartItem.Id = "IT" + OrderCode.RandomString(6);
            cartItem.CustomerId = customer.Id;
            cartItem.ProductDetailId = product_detail_id;
            cartItem.Quantity = quantity;
            cartItem.Is_Check = isCheck;
            cartItem.SavePrice = savePrice;
            cartItem.OrderId = null;

            _db.Carts.Add(cartItem);
            _db.SaveChanges();
        }

        public int checkExistsInDb(string id,string customerId)
        {
            var carts = _db.Carts.ToList().Where(i => i.CustomerId.Equals(customerId)).ToList();
            if(carts !=null && carts.Count >0)
            {
                var cart = _db.Carts.Count();
                for (int i = 0; i < cart; i++)
                {
                    if(_db.Carts.ToList()[i].ProductDetailId.Equals(id) && _db.Carts.ToList()[i].CustomerId.Equals(customerId) && _db.Carts.ToList()[i].OrderId == null)
                    {
                        Debug.WriteLine("Dong thu trong db: "+i);
                        return i;
                    }
                }
            }
            return -1;
        }

        public string checkValidPromo(string customer_id, string promotion_code, int total)
        {
            // Kiem tra promo ton tai
            var promo = _db.Promotions.ToList().SingleOrDefault(p => p.Code.Equals(promotion_code));
            if(promo !=null)
            {
                // Kiem tra han su dung cua promo
                if(promo.StartDate <= DateTime.Now && promo.EndDate >= DateTime.Now)
                {
                    // Kiem tra total pay so voi quy dinh cua promo min order
                    if(promo.MinOrder <= total)
                    {
                        // Kiem tra customer co so huu promo nay
                        var isValidOwner = _db.PromotionDetails.ToList().SingleOrDefault(p => p.PromotionId.Equals(promo.Id) && p.CustomerId.Equals(customer_id));
                        if (isValidOwner != null)
                        {
                            return "valid";
                        }
                        else
                        {
                            return "invalid_owner";
                        }
                    }
                    else
                    {
                        return "invalid_min_order";
                    }
                }
                else
                {
                    return "expired";
                }
            }
            else
            {
                return "invalid_promotion";
            }
            
        }

        public List<Item> findAllCartByCustomerId(string id)
        {
            var carts = _db.Carts.Where(i => i.CustomerId.Equals(id) && i.OrderId == null).ToList();
            var items = new List<Item>();
            foreach (var cart in carts)
            {
                var productDetail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(cart.ProductDetailId));

                var item = new Item();
                item.image = productDetail.Product.Image;
                item.isCheck = cart.Is_Check;
                item.name = productDetail.Product.Name;
                item.price = productDetail.ProductPrice.SalePrice;
                item.product_detail_id = productDetail.ProductDetailId;
                item.quantity = (int)cart.Quantity;
                item.savePrice = cart.SavePrice;
                item.size = productDetail.Size;

                items.Add(item);
            }

            return items;
        }

        public List<OrderProduct> findAllOrderProductOfCustomer(string customer_id)
        {
            return _db.OrderProducts.ToList().Where(i => i.CustomerId.Equals(customer_id)).ToList();
        }

        public List<Promotion> findAllPromotionsOfCustomer(string customer_id)
        {
            var promos = new List<Promotion>();
            var result = _db.PromotionDetails.ToList().Where(i => i.CustomerId.Equals(customer_id));

            foreach (var item in result)
            {
                promos.Add(item.Promotion);
            }

            return promos;
        }

        public ElectronicCommerce.Models.Customer findCustomerById(string id)
        {
            return _db.Customers.ToList().SingleOrDefault(i => i.Id.Equals(id));
        }

        public ElectronicCommerce.Models.Customer findCustomerByMail(string mail)
        {
            return _db.Customers.ToList().SingleOrDefault(i => i.Username.Equals(mail));
        }

        public bool isCustomerFilledInfo(string customer_id)
        {
            var customers = _db.Customers.ToList();
            foreach (var item in customers)
            {
                if (item.Id.Equals(customer_id))
                {
                    if(item.Fullname !=null && item.Username !=null && item.Phone !=null && item.Address !=null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public ElectronicCommerce.Models.Customer isExistMail(string mail)
        {
            var customer = _db.Customers.ToList().SingleOrDefault(p => p.Username.Equals(mail));
            if(customer !=null)
            {
                return customer;
            }
            return null;
        }

        public bool removeItemFromCartInDb(string product_detail_id, string customer_id)
        {
            try
            {
                var cartItem = _db.Carts.ToList().SingleOrDefault(i => i.ProductDetailId.Equals(product_detail_id) && i.CustomerId.Equals(customer_id) && i.OrderId == null);
                _db.Carts.Remove(cartItem);
                _db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public ElectronicCommerce.Models.Customer signInWithGoogle(string mail)
        {
            var customer = isExistMail(mail);
            if(customer != null)
            {
                return customer;
            }
            else
            {
                var newCustomer = new ElectronicCommerce.Models.Customer();
                newCustomer.Id = "MB" + Admin.Helpers.OrderCode.RandomString(6);
                newCustomer.Username = mail;
                newCustomer.Avatar = "temp-avt.jpg";

                _db.Customers.Add(newCustomer);
                _db.SaveChanges();
                return newCustomer;
            }
        }

        public void updateCartInDb(List<Item> items, ElectronicCommerce.Models.Customer customer)
        {
            var carts = new List<Cart>();
            if(_db.Carts.ToList().Count >0)
            {
                carts = _db.Carts.ToList().Where(i => i.CustomerId.Equals(customer.Id) && i.OrderId == null).ToList();
            }
            // Lap qua session
            foreach (var item in items)
            {
                // Neu khach hang chua mua bat ky san pham nao
                // Thi them thang vao dbcart cac san pham tu session
                if(carts.Count <= 0)
                {
                    var itemCart = new Cart();
                    itemCart.Id = "IT" + OrderCode.RandomString(6);
                    itemCart.CustomerId = customer.Id;
                    itemCart.Quantity = item.quantity;
                    itemCart.CustomerId = customer.Id;
                    itemCart.ProductDetailId = item.product_detail_id;
                    itemCart.Is_Check = item.isCheck;
                    itemCart.SavePrice = item.savePrice;

                    _db.Carts.Add(itemCart);
                }
                // Neu co mua thi kiem tra trung roi moi them
                else
                {
                    var isExistItem = carts.SingleOrDefault(i => i.ProductDetailId.Equals(item.product_detail_id) && i.OrderId == null);
                    if (isExistItem != null)
                    {
                        // Kiem tra neu item co so luong > so luong cartdb thi update
                        if (isExistItem.Quantity < item.quantity)
                        {
                            isExistItem.Quantity = item.quantity;
                            _db.Carts.Update(isExistItem);
                        }
                    }
                    // Neu chua ton tai san pham trong gio hang
                    else
                    {
                        var itemCart = new Cart();
                        itemCart.Id = "IT" + OrderCode.RandomString(6);
                        itemCart.CustomerId = customer.Id;
                        itemCart.Quantity = item.quantity;
                        itemCart.CustomerId = customer.Id;
                        itemCart.ProductDetailId = item.product_detail_id;
                        itemCart.Is_Check = item.isCheck;
                        itemCart.SavePrice = item.savePrice;

                        _db.Carts.Add(itemCart);
                    }
                }
            }
            _db.SaveChanges();
        }

        public void updateCustomerInfo(ShippingInformation shipInfo, string customer_id)
        {
            var customer = _db.Customers.ToList().SingleOrDefault(i => i.Id.Equals(customer_id));

            customer.Fullname = shipInfo.FULLNAME;
            customer.Username = shipInfo.MAIL;
            customer.Phone = shipInfo.PHONE;
            customer.Address = shipInfo.ADDRESS;

            _db.Customers.Update(customer);
            _db.SaveChangesAsync();
        }

        public void updateIsCheckCartInDb(string product_tail_id, bool isCheck, string customer_id)
        {
            var carts = _db.Carts.ToList().Where(i => i.CustomerId.Equals(customer_id)).ToList();
            if (carts != null && carts.Count > 0)
            {
                var cart = _db.Carts.Count();
                for (int i = 0; i < cart; i++)
                {
                    if (_db.Carts.ToList()[i].ProductDetailId.Equals(product_tail_id) && _db.Carts.ToList()[i].CustomerId.Equals(customer_id) && _db.Carts.ToList()[i].OrderId == null)
                    {
                        _db.Carts.ToList()[i].Is_Check = isCheck;
                        _db.SaveChanges();
                        return;
                    }
                }
            }
        }

        public void updateQuantityCartInDb(int index,string action)
        {
            var cartItem = _db.Carts.ToList().ElementAt(index);
            if(action.Equals("plus"))
            {
                cartItem.Quantity++;
            }
            else if(action.Equals("minus"))
            {
                cartItem.Quantity--;
            }
            _db.SaveChanges();
        }
    }
}
