using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.BLL
{
    public class ExceptionUser : Exception
    {
        public ExceptionUser() { }
        public ExceptionUser(string messege) : base (messege) { } 
    }
}
