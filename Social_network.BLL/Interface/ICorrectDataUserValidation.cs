using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Social_network.BLL.Models;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.Intarface
{
    public interface ICorrectDataUserValidation
    {
        Task<User> UserCanLOgINAccount(ForLoginUser user);
        Task<User> GetUserByIdAsync(Guid id);
    }
}
