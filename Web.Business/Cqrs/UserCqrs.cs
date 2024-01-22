using Base.Response;
using MediatR;
using Web.Data.Entity;
using Web.Schema;

namespace Web.Business.Cqrs;

public record GetAllUsersQuery() : IRequest<ApiResponse<List<UserResponse>>>;
public record GetUserByIdQuery(int UserId) : IRequest<ApiResponse<UserResponse>>;
public record CreateUserCommand(UserRequest Model) : IRequest<ApiResponse<UserResponse>>;
public record UpdateUserCommand(int UserId,UserRequest Model) : IRequest<ApiResponse<UserResponse>>;
public record DeleteUserCommand(int UserId) : IRequest<ApiResponse>;
