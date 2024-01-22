using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Web.Business.Cqrs;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Query
{
    public class PortfolioQueryHandler :
        IRequestHandler<GetAllPortfoliosQuery, ApiResponse<List<PortfolioResponse>>>,
        IRequestHandler<GetPortfolioByIdQuery, ApiResponse<PortfolioResponse>>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public PortfolioQueryHandler(TradeDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<PortfolioResponse>>> Handle(GetAllPortfoliosQuery request, CancellationToken cancellationToken)
        {
            var portfolios = await _dbContext.Set<Portfolio>()
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<Portfolio>, List<PortfolioResponse>>(portfolios);

            return new ApiResponse<List<PortfolioResponse>>(mapped);
        }

        public async Task<ApiResponse<PortfolioResponse>> Handle(GetPortfolioByIdQuery request, CancellationToken cancellationToken)
        {
            var portfolio = await _dbContext.Set<Portfolio>()
                .FirstOrDefaultAsync(p => p.Id == request.PortfolioId, cancellationToken);

            if (portfolio == null)
                return new ApiResponse<PortfolioResponse>("Portfolio not found");

            var mapped = _mapper.Map<Portfolio, PortfolioResponse>(portfolio);

            return new ApiResponse<PortfolioResponse>(mapped);
        }
    }
}
