using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Social_network.DAL.Models
{
    public class Message
    {
        [Key]
        public Guid MessegeID { get; set; } = Guid.NewGuid();

        public Guid ToUser { get; set; }

        public Guid FromUser { get; set; }

        public string Text { get; set; }
    }
}
