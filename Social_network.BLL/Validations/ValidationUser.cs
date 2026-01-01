using Microsoft.Identity.Client;
using Social_network.BLL.Intarface;
using Social_network.DAL.Interface;
using Social_network.DAL.Models;
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

        public async Task<bool> UpdateUser(User newUserm)
        {
            bool result = true;

            if (string.IsNullOrEmpty(newUserm.Email))
                result = false;

            if (string.IsNullOrEmpty(newUserm.FirstName))
                result = false;

            if (string.IsNullOrEmpty(newUserm.LastName))
                result = false;

            if (newUserm.password.Length < 8)
                result = false;

            if (newUserm.BirthDate == null)
                result = false;

            var EmailIsExist = await _userRepo.GetUserByEmail(newUserm.Email);

            if(!(EmailIsExist == null || EmailIsExist.Id == newUserm.Id))
                result = false;

            if(result)
            {
                await _userRepo.UpdateUser(newUserm, newUserm.Id);
            }
            return result;
        }

    }
}
