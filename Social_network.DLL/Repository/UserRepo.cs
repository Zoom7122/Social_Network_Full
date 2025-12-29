using Microsoft.EntityFrameworkCore;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Social_network.DAL.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ContextSocial_Network_Context _context;

        public UserRepo(ContextSocial_Network_Context contex)
        {
            _context = contex;

        }

        public async Task<User[]> GetAllUsers()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var us = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return us;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RegisterUser(User user)
        {
            var entry = _context.Entry(user);
            if (entry.State == EntityState.Detached)
                await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }
    }
}
