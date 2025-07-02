using AutoMapper;
using ItemsStoreWebAPI.DTOs.TV;
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
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.StockItem, opt => opt.MapFrom(src => new StockItemEntity
                {
                    Id = src.Id,
                    Name = src.Name,
                    Description = src.Description,
                    ReleasedYear = src.ReleasedYear,
                    Price = src.Price,
                    InStock = src.InStock,
                    AddedAt = src.AddedAt,
                    ModifiedAt = src.ModifiedAt
                }))
                .ForMember(dest => dest.StockItemId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<TvEntity, TV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StockItem.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.StockItem.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.StockItem.Description))
                .ForMember(dest => dest.ReleasedYear, opt => opt.MapFrom(src => src.StockItem.ReleasedYear))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.StockItem.Price))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.StockItem.InStock))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.StockItem.AddedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.StockItem.ModifiedAt))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency));
        }
    }
}