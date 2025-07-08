using AutoMapper;
using ItemsStoreWebAPI.DTOs.Mobile;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Mappings
{
    public class MobileProfile : Profile
    {
        public MobileProfile()
        {
            CreateMap<RequestMobileDto, Mobile>();
            CreateMap<ResponseMobileDto, Mobile>();
            CreateMap<Mobile, ResponseMobileDto>();
            
            CreateMap<Mobile, MobileEntity>()
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.BatteryCapacity, opt => opt.MapFrom(src => src.BatteryCapacity))
                .ForMember(dest => dest.RAM, opt => opt.MapFrom(src => src.RAM))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage))
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
            
            CreateMap<MobileEntity, Mobile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StockItem.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.StockItem.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.StockItem.Description))
                .ForMember(dest => dest.ReleasedYear, opt => opt.MapFrom(src => src.StockItem.ReleasedYear))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.StockItem.Price))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.StockItem.InStock))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.StockItem.AddedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.StockItem.ModifiedAt))
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.BatteryCapacity, opt => opt.MapFrom(src => src.BatteryCapacity))
                .ForMember(dest => dest.RAM, opt => opt.MapFrom(src => src.RAM))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage));
        }
    }
}