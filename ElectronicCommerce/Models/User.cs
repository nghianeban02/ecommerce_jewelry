using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class User
    {
        public User()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public DateTime Dob { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
