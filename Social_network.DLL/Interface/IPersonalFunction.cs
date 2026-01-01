using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Interface
{
    public interface IPersonalFunction
    {
        Task<bool> AddToFriends(Guid id_toAdd, Guid id_user);
    }
}
