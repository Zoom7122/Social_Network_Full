using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Intarface
{
    public interface IValidationUser
    {
        Task AccreptUser(User user);

        Task<bool> UpdateUser(User newUserm);
    }
}
