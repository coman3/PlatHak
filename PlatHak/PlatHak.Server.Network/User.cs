using System;
using System.ComponentModel.DataAnnotations;

namespace PlatHak.Server.Network
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime LastLogin { get; set; }
        public string Password { get; set; }
    }
}
