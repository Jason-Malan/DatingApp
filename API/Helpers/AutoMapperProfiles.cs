using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PlatformUser, FrontendUserDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.Value.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();
            CreateMap<MemberUpdateDto, PlatformUser>();
            CreateMap<RegisterDto, PlatformUser>();
        }
    }
}
