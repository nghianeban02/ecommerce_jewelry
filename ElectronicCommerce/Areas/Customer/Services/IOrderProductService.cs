using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface IOrderProductService
    {
        public string NonCustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo, double total);

        public string NonCustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo, double total);

        public string CustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo, ElectronicCommerce.Models.Customer customer, string? code, double total);

        public string CustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo, ElectronicCommerce.Models.Customer customer, string? code, double total);

        public void UpdateQuantityCancelOrder(OrderProduct order);
    }
}
