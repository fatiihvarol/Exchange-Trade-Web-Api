using AutoMapper;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<TradeRequest, TradeResponse>();
        CreateMap<TradeRequest, TradeLog>();
    }
}