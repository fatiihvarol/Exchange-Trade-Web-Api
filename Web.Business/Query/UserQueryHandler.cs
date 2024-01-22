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
    public class UserQueryHandler :
        IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserResponse>>>,
        IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
    {
        private readonly TradeDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserQueryHandler(TradeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _dbContext.Set<User>()
                    .ToListAsync(cancellationToken);

                var mappedUsers = _mapper.Map<List<User>, List<UserResponse>>(users);

                return new ApiResponse<List<UserResponse>>(mappedUsers);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<UserResponse>>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Set<User>()
                    .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                if (user == null)
                    return new ApiResponse<UserResponse>("User not found");

                var mappedUser = _mapper.Map<User, UserResponse>(user);

                return new ApiResponse<UserResponse>(mappedUser);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserResponse>($"An error occurred: {ex.Message}");
            }
        }
    }
}
