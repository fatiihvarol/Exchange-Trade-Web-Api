using Base.Response;
using MediatR;
using Web.Schema;

namespace Web.Business.Cqrs;

public record CreateBuyTradeCommand(TradeRequest Model) : IRequest<ApiResponse<TradeResponse>>;
public record CreateSellTradeCommand(TradeRequest Model) : IRequest<ApiResponse<TradeResponse>>;

public record GetAllTradesQuery() : IRequest<ApiResponse<List<TradeResponse>>>;
public record GetTradeByIdQuery(int Id) : IRequest<ApiResponse<TradeResponse>>;
