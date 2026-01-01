using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Interface
{
    public interface IMessegeRepo
    {
        Task<bool> EnterMessege(Guid fromId, Guid toId, string txt);

        Task<List<Message>> GetAllMessegeUserByIDToWriteHim(Guid userID);

        Task<List<Message>> GetAllMessegeUserByIDToWroteHim(Guid userID);

        Task<Message> GetMessegeById(Guid mesegeID);

    }
}
