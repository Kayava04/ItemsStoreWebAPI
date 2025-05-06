using AutoMapper;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Mappings
{
    public class TvProfile : Profile
    {
        public TvProfile()
        {
            CreateMap<RequestTvDto, TV>();
            CreateMap<ResponseTvDto, TV>();
            CreateMap<TV, ResponseTvDto>();

            CreateMap<TV, TvEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.ReleasedYear, opt => opt.MapFrom(src => src.ReleasedYear))
                .ForPath(dest => dest.StockItem.Price, opt => opt.MapFrom(src => src.Price))
                .ForPath(dest => dest.StockItem.InStock, opt => opt.MapFrom(src => src.InStock));
            
            CreateMap<TvEntity, TV>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.StockItem.Price))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.StockItem.InStock))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.StockItem.AddedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.StockItem.ModifiedAt));
        }
    }
}