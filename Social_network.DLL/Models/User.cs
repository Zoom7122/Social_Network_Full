using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Social_network.DAL.Models
{
    [Table("Usrers")]
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string password { get; set; }
        public string Email { get; set; }
        public List<Guid>? FriendsID { get; set; } = new List<Guid>();
    }
}
