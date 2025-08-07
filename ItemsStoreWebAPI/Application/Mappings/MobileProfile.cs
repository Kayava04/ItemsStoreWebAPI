using AutoMapper;
using ItemsStoreWebAPI.Application.DTOs.Mobile;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Domain.Entities;

namespace ItemsStoreWebAPI.Application.Mappings
{
    public class MobileProfile : Profile
    {
        public MobileProfile()
        {
            CreateMap<RequestMobileDto, Mobile>();
            CreateMap<ResponseMobileDto, Mobile>();
            CreateMap<Mobile, ResponseMobileDto>();
            
            CreateMap<Mobile, MobileEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.BatteryCapacity, opt => opt.MapFrom(src => src.BatteryCapacity))
                .ForMember(dest => dest.RAM, opt => opt.MapFrom(src => src.RAM))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage))
                // .ForMember(dest => dest.StockItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StockItem, opt => opt.MapFrom(src => src));
                
            
            CreateMap<MobileEntity, Mobile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OS, opt => opt.MapFrom(src => src.OS))
                .ForMember(dest => dest.ScreenSize, opt => opt.MapFrom(src => src.ScreenSize))
                .ForMember(dest => dest.BatteryCapacity, opt => opt.MapFrom(src => src.BatteryCapacity))
                .ForMember(dest => dest.RAM, opt => opt.MapFrom(src => src.RAM))
                .ForMember(dest => dest.Storage, opt => opt.MapFrom(src => src.Storage))
                // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StockItem.Id))
                .IncludeMembers(src => src.StockItem);
        }
    }
}