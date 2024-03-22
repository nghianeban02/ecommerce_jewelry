using System;
using System.Collections.Generic;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class UserService:IUserService
    {
        private IBaseRepository<User> _baseRepo;
        private DatabaseContext _db;

        public UserService(IBaseRepository<User> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public bool checkUsernameExists(string username)
        {
            var isExists = _baseRepo.GetAll().ToList().SingleOrDefault(i => i.Username.Equals(username));
            if(isExists != null)
            {
                return true;
            }
            return false;
        }

        public bool checkValidUser(string username, string password)
        {
            var account = _baseRepo.GetAll().ToList().SingleOrDefault(i => i.Username.Equals(username));
            var isValid = BCrypt.Net.BCrypt.Verify(password, account.Password);
            if (isValid)
            {
                return true;
            }
            return false;
        }

        public List<User> findAllUserByRole(string role_id)
        {
            return _baseRepo.GetAll().ToList().Where(i => i.IdRole.Equals(role_id)).ToList();
        }
    }
}
