using Microsoft.EntityFrameworkCore;
using Social_network.DAL.Interface;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Repository
{
    public class MessegeRepo : IMessegeRepo
    {
        private readonly ContextSocial_Network_Context _context;
        private readonly IUserRepo _userRepo;


        public MessegeRepo(ContextSocial_Network_Context context,IUserRepo userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        public async Task<bool> EnterMessege(Guid fromId, Guid toId, string txt)
        {
            bool answer = await _userRepo.IfUsersAreFriendsByID(fromId, toId);

            if (answer)
            {
                Message message = new Message()
                {
                    FromUser = fromId,
                    ToUser = toId,
                    Text = txt
                };

                var entry = _context.Entry(message);
                if (entry.State == EntityState.Detached)
                {
                    await _context.Messeges.AddAsync(message);

                    var userFrom =await _userRepo.GetUserById(fromId);
                    var userTo =await _userRepo.GetUserById(toId);

                    userFrom.MessegesFromUser.Add(message.MessegeID);
                    userTo.MessegesToUser.Add(message.MessegeID);

                }
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;

        }

        //написал он
        public async Task<List<Message>> GetAllMessegeUserByIDToWriteHim(Guid userID)
        { 
            return await _context.Messeges.Where(x => x.FromUser == userID).ToListAsync();
        }

        //Написали ему
        public async Task<List<Message>> GetAllMessegeUserByIDToWroteHim(Guid userID)
        {
            return await _context.Messeges.Where(x => x.ToUser == userID).ToListAsync();
        }

        public async Task<Message> GetMessegeById(Guid mesegeID)
        {
            return await _context.Messeges.FirstOrDefaultAsync(x => x.MessegeID == mesegeID);
        }
    }
}
