using AutoMapper;
using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Business.Cqrs;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Query
{
    public class TradeQueryHandler :
        IRequestHandler<GetAllTradesQuery, ApiResponse<List<TradeResponse>>>,
        IRequestHandler<GetTradeByIdQuery, ApiResponse<TradeResponse>>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public TradeQueryHandler(TradeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<TradeResponse>>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var trades = await _dbContext.Set<TradeLog>()
                    .ToListAsync(cancellationToken);

                var mappedTrades = _mapper.Map<List<TradeLog>, List<TradeResponse>>(trades);

                return new ApiResponse<List<TradeResponse>>(mappedTrades);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TradeResponse>>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TradeResponse>> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var trade = await _dbContext.Set<TradeLog>()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (trade == null)
                    return new ApiResponse<TradeResponse>("Trade not found");

                var mappedTrade = _mapper.Map<TradeLog, TradeResponse>(trade);

                return new ApiResponse<TradeResponse>(mappedTrade);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TradeResponse>($"An error occurred: {ex.Message}");
            }
        }
    }
}
