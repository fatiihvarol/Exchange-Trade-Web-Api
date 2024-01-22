using AutoMapper;
using Base.Enum;
using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Business.Cqrs;
using Web.Business.Service;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Command;

public class TradeLogCommandHandler:
    IRequestHandler<CreateBuyTradeCommand, ApiResponse<TradeResponse>>,
    IRequestHandler<CreateSellTradeCommand,ApiResponse<TradeResponse>>

{
    private readonly TradeDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UpdateSharePrice _updateSharePrice;


    public TradeLogCommandHandler(TradeDbContext dbContext,IMapper mapper,UpdateSharePrice updateSharePrice)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _updateSharePrice = updateSharePrice;
    }

    public async Task<ApiResponse<TradeResponse>> Handle(CreateBuyTradeCommand request, CancellationToken cancellationToken)
    {
        var portfolio = await _dbContext.Set<Portfolio>()
            .FirstOrDefaultAsync(x => x.Id == request.Model.PortfolioId, cancellationToken);

        if (portfolio is null)
            return new ApiResponse<TradeResponse>("portfolio does not exist");

        var share = await _dbContext.Set<Share>()
            .FirstOrDefaultAsync(x => x.Id == request.Model.ShareId, cancellationToken);

        if (share is null)
            return new ApiResponse<TradeResponse>("share does not exist");

        if (portfolio.TotalBalance < share.CurrentPrice * request.Model.Quantity)
            return new ApiResponse<TradeResponse>("insufficient balance");

        PortfolioItem item = new PortfolioItem()
        {
            PortfolioId = request.Model.PortfolioId,
            Quantity = request.Model.Quantity,
            ShareId = request.Model.ShareId,
            InsertDate = DateTime.Now
        };

        // Add TradeLog to the context
        var tradeLog = _mapper.Map<TradeRequest, TradeLog>(request.Model);
        tradeLog.Type = TradeType.Buy;
        tradeLog.InsertDate = DateTime.Now;

        await _dbContext.AddAsync(tradeLog,cancellationToken);
        await _dbContext.AddAsync(item,cancellationToken);

        portfolio.TotalBalance -= share.CurrentPrice * request.Model.Quantity;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<TradeRequest, TradeResponse>(request.Model);
        mapped.Type = TradeType.Buy;

        
        await _updateSharePrice.UpdateSharePriceAfterBuy(request.Model.ShareId, request.Model.Quantity);
        return new ApiResponse<TradeResponse>(mapped);
    }


    public async Task<ApiResponse<TradeResponse>> Handle(CreateSellTradeCommand request, CancellationToken cancellationToken)
    {
        var portfolio =await _dbContext.Set<Portfolio>()
            .FirstOrDefaultAsync(x=>x.Id==request.Model.PortfolioId,cancellationToken);
        if (portfolio is null)
            return new ApiResponse<TradeResponse>("portfolio does not exist");
        
        var portfolioItem =await _dbContext.Set<PortfolioItem>()
            .FirstOrDefaultAsync(x=>x.ShareId==request.Model.ShareId,cancellationToken);
        
        if (portfolioItem is null)
            return new ApiResponse<TradeResponse>("share does not exist in your portfolio");
        
        if (portfolioItem.Quantity<request.Model.Quantity)
            return new ApiResponse<TradeResponse>("you dont have enough share's quantity");
        
        var share =await _dbContext.Set<Share>()
            .FirstOrDefaultAsync(x=>x.Id==request.Model.ShareId,cancellationToken);

        share.TotalAmount += request.Model.Quantity;

        portfolioItem.Quantity -= request.Model.Quantity;
        portfolio.TotalBalance += share.CurrentPrice * request.Model.Quantity;


       await _dbContext.SaveChangesAsync(cancellationToken);

       var mapped = _mapper.Map<TradeRequest, TradeResponse>(request.Model);
       mapped.Type = TradeType.Sell;

       
       await _updateSharePrice.UpdateSharePriceAfterSell(request.Model.ShareId, request.Model.Quantity);

       return new ApiResponse<TradeResponse>(mapped);
       

    }
    
    
    
}