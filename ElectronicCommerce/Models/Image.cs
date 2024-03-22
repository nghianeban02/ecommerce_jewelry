using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Image
    {
        public string Id { get; set; }
        public string NameImages { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
