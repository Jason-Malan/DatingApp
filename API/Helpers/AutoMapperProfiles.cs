using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Models.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, FrontendUserDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.Value.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();
            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            // TODO: Map SenderPhotoUrl to messageDto in logic
            CreateMap<Message, MessageDto>();
            //CreateMap<MessageDto, Message>();
        }
    }
}
