using API.Extensions;
using API.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Entities
{
    public class User
    {
        public User()
        {
            MessagesSent = new HashSet<Message>();
            MessagesReceived = new HashSet<Message>();
        }

        public int? Id { get; set; }
        public string? UserName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastActive { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
