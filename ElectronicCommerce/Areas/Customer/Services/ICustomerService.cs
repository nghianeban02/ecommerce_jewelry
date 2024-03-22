using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface ICustomerService
    {
        public ElectronicCommerce.Models.Customer isExistMail(string mail);

        public ElectronicCommerce.Models.Customer signInWithGoogle(string mail);

        public void updateCartInDb(List<Item> items, ElectronicCommerce.Models.Customer customer);
        public void addToCartInDb(int savePrice,Boolean isCheck,string product_detail_id, ElectronicCommerce.Models.Customer customer, int quantity);

        public int checkExistsInDb(string id,string customerId);

        public void updateQuantityCartInDb(int index, string action);

        public List<Item> findAllCartByCustomerId(string id);

        public ElectronicCommerce.Models.Customer findCustomerById(string id);

        public ElectronicCommerce.Models.Customer findCustomerByMail(string mail);

        public bool removeItemFromCartInDb(string product_detail_id, string customer_id);

        public void updateIsCheckCartInDb(string product_tail_id, Boolean isCheck, string customer_id);

        public bool isCustomerFilledInfo(string customer_id);

        public void updateCustomerInfo(ShippingInformation shipInfo, string customer_id);

        public string checkValidPromo(string customer_id, string promotion_code, int total);

        public List<Promotion> findAllPromotionsOfCustomer(string customer_id);

        public List<OrderProduct> findAllOrderProductOfCustomer(string customer_id);

    }
}
