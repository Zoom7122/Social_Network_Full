using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Interface
{
    public interface IUserRepo
    {
        Task<User[]> GetAllUsers();

        Task<User> GetUserById(Guid id);

        Task<User> GetUserByEmail(string email);

        Task RegisterUser(User user);

        Task<List<User>> ListFriendsOfUser(List<Guid> listIdFriends);

        Task<bool> IfUsersAreFriendsByID(Guid user1ID, Guid user2ID);

        Task<bool> DeletedFromFriends(Guid userID, Guid FriendsId);

        Task UpdateUser(User user, Guid userID);
    }
}
