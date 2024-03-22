using ElectronicCommerce.Models;
using System.Diagnostics;

namespace ElectronicCommerce.Areas.Admin.ViewModels
{
    public class CreateStoreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Image { get; set; }
        public Store Convert(CreateStoreViewModel vm)
        {
            return new Store 
            {
                Name = vm.Name,
                District = vm.District,
                Address = vm.Address,
                City = vm.City,
                Description = vm.Description,
                Image = vm.Image
            };
        }
    }
}
