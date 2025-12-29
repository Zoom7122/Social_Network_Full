using Social_network.BLL.Intarface;
using Social_network.BLL.Models;
using Social_network.DAL.Models;
using Social_network.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Validations
{
    public class CorrectDataUserValidation : ICorrectDataUserValidation
    {
        private readonly IUserRepo _userRepo;
        public CorrectDataUserValidation(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User> UserCanLOgINAccount(ForLoginUser user)
        {
            var us = await _userRepo.GetUserByEmail(user.Email);
            if (us != null && us.password == user.Password)
            {
                return us;
            }
            else return null;
        }
    }
}
