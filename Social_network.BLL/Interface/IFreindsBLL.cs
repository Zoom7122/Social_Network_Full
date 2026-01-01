using Social_network.BLL.ClassToViewFriends;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Intarface
{
    public interface IFreindsBLL
    {
        Task<List<Friend>> FindFriendsUser(List<Guid> listGuidId);

        Task<bool> GetuserByEmailToFriends(string email, Guid id);

        Task<bool> DeletedUserBLL(Guid id, Guid friendsID);
    }
}
