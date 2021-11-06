using AutoMapper;
using DataLayer.Entities;
using Services.Dtos;

namespace Services.AutomapperProfiles
{
    public class FacebookProfiles : Profile
    {

        public FacebookProfiles()
        {
            CreateMap<FacebookPost, FacebookPostDto>()
                .ForMember(dest => dest.MediaFile, opt => opt.MapFrom(src => src.MediaFile.Trim()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.Trim()))
                .ReverseMap();
        }

    }
}
