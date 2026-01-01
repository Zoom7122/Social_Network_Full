using Microsoft.EntityFrameworkCore;
using Social_network.DAL.Interface;
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

        public async Task<bool> DeletedFromFriends(Guid userID, Guid FriendsId)
        {
            var mainUser = await GetUserById(userID);
            if (mainUser == null) 
                return false;
            var friends = await GetUserById(FriendsId);
            if (friends == null) 
                return false;

            if (mainUser.FriendsID.Contains(friends.Id))
            {
                mainUser.FriendsID.Remove(FriendsId);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
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

        public async Task<bool> IfUsersAreFriendsByID(Guid user1ID, Guid user2ID)
        {
            var user1 = await GetUserById(user1ID);
            if (user1 == null)
                return false;

            var user2 = await GetUserById(user2ID);
            if (user2 == null)
                return false;

            if (user1.FriendsID.Contains(user2ID) && user2.FriendsID.Contains(user1ID))
                return true;

            return false;
        }

        public async Task<List<User>> ListFriendsOfUser(List<Guid> listIdFriends)
        {
            List<User> users = new List<User>();

            for(int i=0;i < listIdFriends.Count; i++)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == listIdFriends[i]);
                if (user != null)
                    users.Add(user);
            }
            return users;
        }

        public async Task RegisterUser(User user)
        {
            var entry = _context.Entry(user);
            if (entry.State == EntityState.Detached)
                await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user, Guid userID)
        {
            var existingUser = await GetUserById(userID);

            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.BirthDate = user.BirthDate;
            existingUser.Email = user.Email;

            await _context.SaveChangesAsync();
        }
    }
}
