using AutoMapper;
using ItemsStoreWebAPI.Application.DTOs.TV;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Domain.Entities;

namespace ItemsStoreWebAPI.Application.Mappings
{
    public class TvProfile : Profile
    {
        public TvProfile()
        {
            CreateMap<RequestTvDto, TV>();
            CreateMap<ResponseTvDto, TV>();
            
            CreateMap<TV, RequestTvDto>();
            CreateMap<TV, ResponseTvDto>();

            CreateMap<TV, TvEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.StockItem, opt => opt.MapFrom(src => src));
            
            CreateMap<TvEntity, TV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .IncludeMembers(src => src.StockItem);
        }
    }
}