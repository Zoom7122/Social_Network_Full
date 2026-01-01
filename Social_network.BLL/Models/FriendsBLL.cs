using Social_network.BLL.ClassToViewFriends;
using Social_network.BLL.Intarface;
using Social_network.DAL.Interface;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Models
{
    public class FriendsBLL : IFreindsBLL
    {
        private readonly IUserRepo _userRepo;
        private readonly IPersonalFunction _personalFunction;
        public FriendsBLL(IUserRepo userRepo, IPersonalFunction personalFunction,
            IMessegeRepo messegeRepo)
        {
            _userRepo = userRepo;
            _personalFunction = personalFunction;
        }

        public async Task<bool> DeletedUserBLL(Guid mainID, Guid frendsID)
        {
            return await _userRepo.DeletedFromFriends(mainID, frendsID);
        }

        public async Task<List<Friend>> FindFriendsUser(List<Guid> listGuidId)
        {
            List<Friend> friends = new List<Friend>();

            for (int i = 0; i < listGuidId.Count; i++)
            {
                var user = await _userRepo.GetUserById(listGuidId[i]);

                if (user != null)
                {
                    var friend = new Friend()
                    {
                        FriendID = user.Id,
                        FullName = user.FirstName + " " + user.LastName,
                        Email = user.Email,
                    };

                    friends.Add(friend);
                }
            }

            return friends;
        }

        public async Task<bool> GetuserByEmailToFriends(string email, Guid main_id)
        {
            var userToAdd =  await _userRepo.GetUserByEmail(email);

            var mainUser = await _userRepo.GetUserById(main_id);

            if (userToAdd != null && mainUser != null)
            {
                return await _personalFunction.AddToFriends(mainUser.Id, userToAdd.Id);
            }
            return false;
        }
    }
}
