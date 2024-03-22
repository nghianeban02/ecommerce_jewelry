using System;
using ElectronicCommerce.Areas.Admin.ViewModels;
using System.Collections.Generic;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface IStoreService
    {
        public List<Store> getAll();

    }
}
