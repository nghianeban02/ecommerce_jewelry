using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface ICategoryProductService
    {
        //xem danh sách danh mục parent
        public List<CategoryProduct> findAllParentCategory();



    }
}
