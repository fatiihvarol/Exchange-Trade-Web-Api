// TradeLogCommandHandler.cs
using AutoMapper;
using Base.Enum;
using Base.Response;
using MediatR;

using Web.Business.Cqrs;
using Web.Business.Service;
using Web.Business.Service.BaseHelperService;
using Web.Data.DbContext;
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
        private readonly BaseHelperService _baseHelperService;

        public TradeLogCommandHandler(TradeDbContext dbContext, IMapper mapper, UpdateSharePrice updateSharePrice, BaseHelperService baseHelperService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _updateSharePrice = updateSharePrice ?? throw new ArgumentNullException(nameof(updateSharePrice));
            _baseHelperService = baseHelperService ?? throw new ArgumentNullException(nameof(baseHelperService));
        }

        public async Task<ApiResponse<TradeResponse>> Handle(CreateBuyTradeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var portfolio = await _baseHelperService.GetPortfolioAsync(request.Model.PortfolioId, cancellationToken, "portfolio does not exist");
                var share = await _baseHelperService.GetShareAsync(request.Model.ShareId, cancellationToken, "share does not exist");

                if (portfolio.TotalBalance < share.CurrentPrice * request.Model.Quantity)
                    return new ApiResponse<TradeResponse>("insufficient balance");

                var portfolioItem = await _baseHelperService.GetPortfolioItemAsync(request.Model.ShareId, cancellationToken);

                _baseHelperService.UpdatePortfolioItem(portfolioItem, request.Model);

                var tradeLog = _baseHelperService.CreateTradeLog(request.Model, TradeType.Buy);
                await _dbContext.AddAsync(tradeLog, cancellationToken);

                _baseHelperService.UpdatePortfolio(portfolio, share, request.Model.Quantity);

                await _dbContext.SaveChangesAsync(cancellationToken);

                var mapped = _baseHelperService.MapTradeRequestToResponse(request.Model, TradeType.Buy);
                await _updateSharePrice.UpdateSharePriceAfterBuy(request.Model.ShareId, request.Model.Quantity);

                return new ApiResponse<TradeResponse>(mapped);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TradeResponse>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TradeResponse>> Handle(CreateSellTradeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var portfolio = await _baseHelperService.GetPortfolioAsync(request.Model.PortfolioId, cancellationToken, "portfolio does not exist");
                var portfolioItem = await _baseHelperService.GetPortfolioItemAsync(request.Model.ShareId, cancellationToken, "share does not exist in your portfolio");

                if (portfolioItem.Quantity < request.Model.Quantity)
                    return new ApiResponse<TradeResponse>("you don't have enough share's quantity");

                var share = await _baseHelperService.GetShareAsync(request.Model.ShareId, cancellationToken);

                share.TotalAmount += request.Model.Quantity;
                portfolioItem.Quantity -= request.Model.Quantity;
                portfolio.TotalBalance += share.CurrentPrice * request.Model.Quantity;

                var tradeLog = _baseHelperService.CreateTradeLog(request.Model, TradeType.Sell);
                await _dbContext.AddAsync(tradeLog, cancellationToken);

                var mapped = _baseHelperService.MapTradeRequestToResponse(request.Model, TradeType.Sell);
                await _updateSharePrice.UpdateSharePriceAfterSell(request.Model.ShareId, request.Model.Quantity);

                await _dbContext.SaveChangesAsync(cancellationToken);
                return new ApiResponse<TradeResponse>(mapped);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TradeResponse>($"An error occurred: {ex.Message}");
            }
        }
    }
}
