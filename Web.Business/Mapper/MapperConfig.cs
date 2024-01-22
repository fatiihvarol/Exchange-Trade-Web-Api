using AutoMapper;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Trade mapping
            CreateMap<TradeRequest, TradeResponse>();
            CreateMap<TradeRequest, TradeLog>();

            // User mapping
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, UserResponse>();

            // Share mapping
            CreateMap<ShareRequest, Share>();
            CreateMap<Share, ShareResponse>();
            CreateMap<ShareRequest, ShareResponse>();

            // Portfolio mapping
            CreateMap<PortfolioRequest, Portfolio>();
            CreateMap<Portfolio, PortfolioResponse>();
            CreateMap<PortfolioRequest, PortfolioResponse>();
        }
    }
}