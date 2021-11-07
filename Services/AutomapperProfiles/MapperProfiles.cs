using AutoMapper;
using DataLayer.Entities;
using Services.Dtos;

namespace Services.AutomapperProfiles
{
    public class MapperProfiles : Profile
    {

        public MapperProfiles()
        {
            /* competitor objects */
            CreateMap<Competitor, CompetitorDto>()
                .ReverseMap();
             
            CreateMap<Competitor, CompetitorWithPosts>()
                .ForPath(dest => dest.Competitor, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ReverseMap();

            /* facebook objects */
            CreateMap<FacebookPost, FacebookPostDto>()
                .ForMember(dest => dest.MediaFile, opt => opt.MapFrom(src => src.MediaFile.Trim()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.Trim()))
                .ReverseMap();
            
            CreateMap<FacebookPost, MetricsFacebookPost>()
                .ForMember(dest => dest.MediaFile, opt => opt.MapFrom(src => src.MediaFile.Trim()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.Trim()))
                .ForMember(dest => dest.Competitor, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<FacebookPost, LightFacebookPostDto>().ReverseMap();

            CreateMap<FacebookPostAddDto, FacebookPost>()
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFile, opt => opt.MapFrom(src => src.MediaFile.Trim()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.Trim()))
                .ReverseMap();
        }
    }
}
