using AutoMapper;
using Base.Enum;
using Microsoft.EntityFrameworkCore;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Service.BaseHelperService;

public class BaseHelperService
{
    private readonly TradeDbContext _dbContext;
    private readonly IMapper _mapper;


    public BaseHelperService(TradeDbContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<Portfolio> GetPortfolioAsync(int portfolioId, CancellationToken cancellationToken, string errorMessage)
    {
        var portfolio = await _dbContext.Set<Portfolio>()
            .FirstOrDefaultAsync(x => x.Id == portfolioId, cancellationToken);

        if (portfolio is null)
            throw new InvalidOperationException(errorMessage);

        return portfolio;
    }
    
    public async Task<Share> GetShareAsync(int shareId, CancellationToken cancellationToken, string errorMessage = null)
    {
        var share = await _dbContext.Set<Share>()
            .FirstOrDefaultAsync(x => x.Id == shareId, cancellationToken);

        if (share is null && errorMessage != null)
            throw new InvalidOperationException(errorMessage);

        return share;
    }
    
    public async Task<PortfolioItem> GetPortfolioItemAsync(int shareId, CancellationToken cancellationToken, string errorMessage = null)
    {
        var portfolioItem = await _dbContext.Set<PortfolioItem>()
            .FirstOrDefaultAsync(x => x.ShareId == shareId, cancellationToken);

        if (portfolioItem is null && errorMessage != null)
            throw new InvalidOperationException(errorMessage);

        return portfolioItem;
    }
    
    public void UpdatePortfolioItem(PortfolioItem portfolioItem, TradeRequest model)
    {
        if (portfolioItem is null)
        {
            portfolioItem = new PortfolioItem()
            {
                PortfolioId = model.PortfolioId,
                Quantity = model.Quantity,
                ShareId = model.ShareId,
                InsertDate = DateTime.Now
            };
            _dbContext.AddAsync(portfolioItem);
        }
        else
        {
            portfolioItem.Quantity += model.Quantity;
        }
    }
    
    
    
    public TradeLog CreateTradeLog(TradeRequest model, TradeType tradeType)
    {
        var tradeLog = _mapper.Map<TradeRequest, TradeLog>(model);
        tradeLog.Type = tradeType;
        tradeLog.InsertDate = DateTime.Now;

        return tradeLog;
    }

    public void UpdatePortfolio(Portfolio portfolio, Share share, int quantity)
    {
        portfolio.TotalBalance -= share.CurrentPrice * quantity;
    }

    public TradeResponse MapTradeRequestToResponse(TradeRequest model, TradeType tradeType)
    {
        var mapped = _mapper.Map<TradeRequest, TradeResponse>(model);
        mapped.Type = tradeType;

        return mapped;
    }
}