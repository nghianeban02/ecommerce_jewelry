using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customers = new HashSet<Customer>();
        }

        public string Id { get; set; }
        public string CustomerTypeName { get; set; }
        public int? DiscountValue { get; set; }
        public string DiscountUnit { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
