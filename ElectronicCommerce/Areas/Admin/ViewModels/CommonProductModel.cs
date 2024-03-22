using ElectronicCommerce.Models;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.ViewModels
{
    public class CommonProductModel
    {
        public Product PRODUCT { get; set; }
        public List<ProductDetail> PRODUCT_DETAILS { get; set; }

    }
}
