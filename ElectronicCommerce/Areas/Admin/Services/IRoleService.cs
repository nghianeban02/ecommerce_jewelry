using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Admin.ViewModels;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface IRoleService
    {
        public List<RolesModel> findAllRoleWithTotalUser();
    }
}
