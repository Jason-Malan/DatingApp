﻿using API.Extensions;
using API.Models.Entities;

namespace API.DTOs
{
    public class FrontendUserDto
    {
        public FrontendUserDto()
        {
            Photos = new List<PhotoDto>();
            Likes = new List<UserLike>();
            MessagesSent = new HashSet<Message>();
            MessagesReceived = new HashSet<Message>();
        }

        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? KnownAs { get; set; }
        public int? Age { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastActive { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public IList<PhotoDto>? Photos { get; set; }
        public string? PhotoUrl { get; set; }
        public IList<UserLike>? Likes { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
