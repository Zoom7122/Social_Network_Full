using Microsoft.Identity.Client;
using Social_network.BLL.Intarface;
using Social_network.DAL.Models;
using Social_network.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Validations
{
    public class ValidationUser : IValidationUser
    {
        private readonly IUserRepo _userRepo;
        public ValidationUser(IUserRepo userRepo) 
        {
            _userRepo = userRepo;
        }

        public async Task AccreptUser(User user)
        {
            if (user == null)
                throw new ExceptionUser("User не передан");

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new ExceptionUser("Lastname is null");

            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new ExceptionUser("FirstName is null");

            if (user.BirthDate == null)
                throw new ExceptionUser("BirthDate is null");

            if (user.password.Length < 8)
                throw new ExceptionUser("Пароль должен быть не менее 8 символов)");

            var userForEmail = await _userRepo.GetUserByEmail(user.Email);
            if(userForEmail != null)
                throw new ExceptionUser("Пользователь с таким Email уже зарегистрирован");


            await _userRepo.RegisterUser(user);
        }

    }
}
