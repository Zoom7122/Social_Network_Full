using Microsoft.EntityFrameworkCore;
using Social_network.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL
{
    public class ContextSocial_Network_Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ContextSocial_Network_Context(DbContextOptions<ContextSocial_Network_Context> opt) : base(opt)
        {
            Database.EnsureCreated();
        }
    }
}
