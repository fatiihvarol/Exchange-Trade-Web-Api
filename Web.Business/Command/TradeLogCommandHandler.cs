using AutoMapper;
using Base.Enum;
using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Business.Cqrs;
using Web.Business.Service;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Command
{
    public class TradeLogCommandHandler :
        IRequestHandler<CreateBuyTradeCommand, ApiResponse<TradeResponse>>,
        IRequestHandler<CreateSellTradeCommand, ApiResponse<TradeResponse>>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UpdateSharePrice _updateSharePrice;

        public TradeLogCommandHandler(TradeDbContext dbContext, IMapper mapper, UpdateSharePrice updateSharePrice)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _updateSharePrice = updateSharePrice ?? throw new ArgumentNullException(nameof(updateSharePrice));
        }

        public async Task<ApiResponse<TradeResponse>> Handle(CreateBuyTradeCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await GetPortfolioAsync(request.Model.PortfolioId, cancellationToken, "portfolio does not exist");
            var share = await GetShareAsync(request.Model.ShareId, cancellationToken, "share does not exist");

            if (portfolio.TotalBalance < share.CurrentPrice * request.Model.Quantity)
                return new ApiResponse<TradeResponse>("insufficient balance");

            var portfolioItem = await GetPortfolioItemAsync(request.Model.ShareId, cancellationToken);

            UpdatePortfolioItem(portfolioItem, request.Model);

            var tradeLog = CreateTradeLog(request.Model, TradeType.Buy);
            await _dbContext.AddAsync(tradeLog, cancellationToken);

            UpdatePortfolio(portfolio, share, request.Model.Quantity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var mapped = MapTradeRequestToResponse(request.Model, TradeType.Buy);
            await _updateSharePrice.UpdateSharePriceAfterBuy(request.Model.ShareId, request.Model.Quantity);

            return new ApiResponse<TradeResponse>(mapped);
        }

        public async Task<ApiResponse<TradeResponse>> Handle(CreateSellTradeCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await GetPortfolioAsync(request.Model.PortfolioId, cancellationToken, "portfolio does not exist");
            var portfolioItem = await GetPortfolioItemAsync(request.Model.ShareId, cancellationToken, "share does not exist in your portfolio");

            if (portfolioItem.Quantity < request.Model.Quantity)
                return new ApiResponse<TradeResponse>("you don't have enough share's quantity");

            var share = await GetShareAsync(request.Model.ShareId, cancellationToken);

            share.TotalAmount += request.Model.Quantity;
            portfolioItem.Quantity -= request.Model.Quantity;
            portfolio.TotalBalance += share.CurrentPrice * request.Model.Quantity;

            var tradeLog = CreateTradeLog(request.Model, TradeType.Sell);
            await _dbContext.AddAsync(tradeLog, cancellationToken);
            
           

            var mapped = MapTradeRequestToResponse(request.Model, TradeType.Sell);
            await _updateSharePrice.UpdateSharePriceAfterSell(request.Model.ShareId, request.Model.Quantity);

            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse<TradeResponse>(mapped);
        }

        // Helper methods...

        private async Task<Portfolio> GetPortfolioAsync(int portfolioId, CancellationToken cancellationToken, string errorMessage)
        {
            var portfolio = await _dbContext.Set<Portfolio>()
                .FirstOrDefaultAsync(x => x.Id == portfolioId, cancellationToken);

            if (portfolio is null)
                throw new InvalidOperationException(errorMessage);

            return portfolio;
        }

        private async Task<Share> GetShareAsync(int shareId, CancellationToken cancellationToken, string errorMessage = null)
        {
            var share = await _dbContext.Set<Share>()
                .FirstOrDefaultAsync(x => x.Id == shareId, cancellationToken);

            if (share is null && errorMessage != null)
                throw new InvalidOperationException(errorMessage);

            return share;
        }

        private async Task<PortfolioItem> GetPortfolioItemAsync(int shareId, CancellationToken cancellationToken, string errorMessage = null)
        {
            var portfolioItem = await _dbContext.Set<PortfolioItem>()
                .FirstOrDefaultAsync(x => x.ShareId == shareId, cancellationToken);

            if (portfolioItem is null && errorMessage != null)
                throw new InvalidOperationException(errorMessage);

            return portfolioItem;
        }

        private void UpdatePortfolioItem(PortfolioItem portfolioItem, TradeRequest model)
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

        private TradeLog CreateTradeLog(TradeRequest model, TradeType tradeType)
        {
            var tradeLog = _mapper.Map<TradeRequest, TradeLog>(model);
            tradeLog.Type = tradeType;
            tradeLog.InsertDate = DateTime.Now;

            return tradeLog;
        }

        private void UpdatePortfolio(Portfolio portfolio, Share share, int quantity)
        {
            portfolio.TotalBalance -= share.CurrentPrice * quantity;
        }

        private TradeResponse MapTradeRequestToResponse(TradeRequest model, TradeType tradeType)
        {
            var mapped = _mapper.Map<TradeRequest, TradeResponse>(model);
            mapped.Type = tradeType;

            return mapped;
        }
    }
}
