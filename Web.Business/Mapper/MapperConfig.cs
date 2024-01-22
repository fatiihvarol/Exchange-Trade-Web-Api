using AutoMapper;
using Web.Schema;

namespace Web.Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<TradeRequest, TradeResponse>();
    }
}