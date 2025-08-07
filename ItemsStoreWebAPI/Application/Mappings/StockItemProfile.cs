using AutoMapper;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Domain.Entities;

namespace ItemsStoreWebAPI.Application.Mappings
{
    public class StockItemProfile  : Profile
    {
        public StockItemProfile()
        {
            CreateMap<StockItem, StockItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<StockItemEntity, StockItem>();
            
            CreateMap<StockItemEntity, Mobile>();
            CreateMap<StockItemEntity, TV>();
        }
    }
}