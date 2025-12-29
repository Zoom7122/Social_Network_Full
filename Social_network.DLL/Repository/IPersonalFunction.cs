using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Repository
{
    public interface IPersonalFunction
    {
        Task<bool> AddToFriends(Guid id_toAdd, Guid id_user);
    }
}
