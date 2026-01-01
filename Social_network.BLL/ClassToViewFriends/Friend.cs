using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL.ClassToViewFriends
{
    public class Friend
    {
        public Guid FriendID { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
