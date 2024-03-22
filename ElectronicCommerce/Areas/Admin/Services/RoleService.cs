using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class RoleService : IRoleService
    {
        private DatabaseContext _db;
        public RoleService(DatabaseContext db)
        {
            _db = db;
        }

        public List<RolesModel> findAllRoleWithTotalUser()
        {
            return _db.RolesModels.FromSqlRaw("sp_findall_roles_with_totaluser").ToList();
        }
    }
}
