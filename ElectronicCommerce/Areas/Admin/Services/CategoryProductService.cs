using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class CategoryProductService : ICategoryProductService
    {
        private IBaseRepository<CategoryProduct> _baseRepo;
        public CategoryProductService(IBaseRepository<CategoryProduct> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public List<CategoryProduct> findAllParentCategory()
        {
            return _baseRepo.GetAll().ToList().Where(i => i.ParentId==null).ToList();
        }

    }
}
