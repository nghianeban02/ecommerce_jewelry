using System;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Models
{
    [Serializable]
    public class Item
    {
        public string product_detail_id { get; set; }
        public string product_id { get; set; }
        public string image { get; set; }
        public int size { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public bool isCheck { get; set; }
        public int? savePrice { get; set; }
    }
}
