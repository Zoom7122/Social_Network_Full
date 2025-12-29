using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Repository
{
    public interface IUserRepo
    {
        Task<User[]> GetAllUsers();

        Task<User> GetUserById(Guid id);

        Task<User> GetUserByEmail(string email);

        Task RegisterUser(User user);
    }
}
