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

namespace Web.Business.Command
{
    public class PortfolioCommandHandler :
        IRequestHandler<CreatePortfolioCommand, ApiResponse<PortfolioResponse>>,
        IRequestHandler<UpdatePortfolioCommand, ApiResponse<PortfolioResponse>>,
        IRequestHandler<DeletePortfolioCommand, ApiResponse>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public PortfolioCommandHandler(TradeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PortfolioResponse>> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
        {
            var existingPortfolio = await _dbContext.Set<Portfolio>()
                .FirstOrDefaultAsync(x => x.UserId == request.Model.UserId, cancellationToken);

            if (existingPortfolio != null)
                return new ApiResponse<PortfolioResponse>("Portfolio with the same name already exists");

            var mappedPortfolio = _mapper.Map<PortfolioRequest, Portfolio>(request.Model);

            await _dbContext.AddAsync(mappedPortfolio, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<Portfolio, PortfolioResponse>(mappedPortfolio);
            return new ApiResponse<PortfolioResponse>(response);
        }

        public async Task<ApiResponse<PortfolioResponse>> Handle(UpdatePortfolioCommand request, CancellationToken cancellationToken)
        {
            var existingPortfolio = await _dbContext.Set<Portfolio>()
                .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            if (existingPortfolio == null)
                return new ApiResponse<PortfolioResponse>("Portfolio not found");

            existingPortfolio.TotalBalance = request.Model.TotalBalance;
            
            _dbContext.Update(existingPortfolio);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<Portfolio, PortfolioResponse>(existingPortfolio);
            return new ApiResponse<PortfolioResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            var existingPortfolio = await _dbContext.Set<Portfolio>()
                .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            if (existingPortfolio == null)
                return new ApiResponse("Portfolio not found");

            _dbContext.Remove(existingPortfolio);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse("Portfolio deleted successfully");
        }
    }
}
