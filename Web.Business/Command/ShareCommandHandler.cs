using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Web.Business.Cqrs;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Command
{
    public class ShareCommandHandler :
        IRequestHandler<CreateShareCommand, ApiResponse<ShareResponse>>,
        IRequestHandler<UpdateShareCommand, ApiResponse<ShareResponse>>,
        IRequestHandler<DeleteShareCommand, ApiResponse>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShareCommandHandler(TradeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ShareResponse>> Handle(CreateShareCommand request, CancellationToken cancellationToken)
        {
            // Check if the share with the same name already exists
            var existingShare = await _dbContext.Set<Share>()
                .FirstOrDefaultAsync(x => x.Symbol == request.Model.Symbol, cancellationToken);

            if (existingShare != null)
                return new ApiResponse<ShareResponse>("Share with the same symbol already exists");

            var mappedShare = _mapper.Map<ShareRequest, Share>(request.Model);

            await _dbContext.AddAsync(mappedShare, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<Share, ShareResponse>(mappedShare);
            return new ApiResponse<ShareResponse>(response);
        }

        public async Task<ApiResponse<ShareResponse>> Handle(UpdateShareCommand request, CancellationToken cancellationToken)
        {
            var existingShare = await _dbContext.Set<Share>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (existingShare == null)
                return new ApiResponse<ShareResponse>("Share not found");

            // Update share properties
            existingShare.CurrentPrice = request.Model.CurrentPrice;
            existingShare.Symbol= request.Model.Symbol;
            existingShare.TotalAmount = request.Model.TotalAmount;

            _dbContext.Update(existingShare);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<Share, ShareResponse>(existingShare);
            return new ApiResponse<ShareResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteShareCommand request, CancellationToken cancellationToken)
        {
            var existingShare = await _dbContext.Set<Share>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (existingShare == null)
                return new ApiResponse("Share not found");

            _dbContext.Remove(existingShare);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse("Share deleted successfully");
        }
    }
}
