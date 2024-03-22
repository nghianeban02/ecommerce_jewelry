using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Review
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Rate { get; set; }
        public DateTime? Created_Date { get; set; }
        public bool? Is_Update { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
