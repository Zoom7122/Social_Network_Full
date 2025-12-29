using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Repository
{
    public class PersonalFunction : IPersonalFunction
    {
        private readonly IUserRepo _userRepo;
        private readonly ContextSocial_Network_Context _context;
        public PersonalFunction(IUserRepo userRepo, ContextSocial_Network_Context context) 
        {
            _userRepo = userRepo;
            _context = context;
        }

        public async Task<bool> AddToFriends(Guid id_main, Guid id_toAdd)
        {
            var mainUser = await _userRepo.GetUserById(id_main);
            var userToAdd = await _userRepo.GetUserById(id_toAdd);
            if(mainUser == null || userToAdd == null)
                return false;
            if (!mainUser.FriendsID.Contains(id_toAdd) && id_main != id_toAdd)
                mainUser.FriendsID.Add(id_toAdd);
            else return false;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
