using Base.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Business.Cqrs;
using Web.Schema;

namespace Exchange_Trade_Web_Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TradesController:ControllerBase
{
    private readonly IMediator mediator;

    public TradesController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpPost("Buy")]
  //  [Authorize(Roles = "admin")]
    public async Task<ApiResponse<TradeResponse>> CreateBuyTrade(TradeRequest request)
    {
        
        var operation = new CreateBuyTradeCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpPost("Sell")]
   // [Authorize(Roles = "admin")]
    public async Task<ApiResponse<TradeResponse>> CreateSellTrade(TradeRequest request)
    {
        
        var operation = new CreateSellTradeCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }
   
    [HttpGet]
    // [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<TradeResponse>>> GetAllTrades()
    {
        
        var operation = new GetAllTradesQuery();
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpGet]
    // [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<TradeResponse>> GetTradeById(int id)
    {
        
        var operation = new GetTradeByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }
}