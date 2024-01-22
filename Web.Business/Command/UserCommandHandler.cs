using AutoMapper;
using Base.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Business.Cqrs;
using Web.Data.DbContext;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Command;

public class UserCommandHandler:
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<DeleteUserCommand, ApiResponse>
{
    private readonly TradeDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserCommandHandler(TradeDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.UserName == request.Model.UserName, cancellationToken);

        if (user is not null)
            return new ApiResponse<UserResponse>("username already exists");

        var mappedUser = _mapper.Map<UserRequest, User>(request.Model);

        // Use AddAsync instead of AddRangeAsync for a single entity
        await _dbContext.AddAsync(mappedUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<UserRequest, UserResponse>(request.Model);
        return new ApiResponse<UserResponse>(response);
    }


    public async Task<ApiResponse<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (existingUser == null)
            return new ApiResponse<UserResponse>("User not found");

        // Update user properties with values from the request
        _mapper.Map(request.Model, existingUser);

        // Save changes to the database
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<User, UserResponse>(existingUser);
        return new ApiResponse<UserResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await _dbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (userToDelete == null)
            return new ApiResponse("User not found");

        userToDelete.IsActive = false;

       
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }

}