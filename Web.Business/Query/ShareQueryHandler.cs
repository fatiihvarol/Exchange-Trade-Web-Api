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
    public class ShareQueryHandler :
        IRequestHandler<GetAllSharesQuery, ApiResponse<List<ShareResponse>>>,
        IRequestHandler<GetShareByIdQuery, ApiResponse<ShareResponse>>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShareQueryHandler(TradeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ShareResponse>>> Handle(GetAllSharesQuery request, CancellationToken cancellationToken)
        {
            var shares = await _dbContext.Set<Share>()
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<List<Share>, List<ShareResponse>>(shares);
            return new ApiResponse<List<ShareResponse>>(response);
        }

        public async Task<ApiResponse<ShareResponse>> Handle(GetShareByIdQuery request, CancellationToken cancellationToken)
        {
            var share = await _dbContext.Set<Share>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (share == null)
                return new ApiResponse<ShareResponse>("Share not found");

            var response = _mapper.Map<Share, ShareResponse>(share);
            return new ApiResponse<ShareResponse>(response);
        }
    }
}