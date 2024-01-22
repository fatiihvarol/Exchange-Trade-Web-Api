using Base.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Business.Cqrs;
using Web.Schema;

namespace Exchange_Trade_Web_Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ApiResponse<UserResponse>> CreateUser(UserRequest request)
    {
        var operation = new CreateUserCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpPut]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<UserResponse>> UpdateUser(int id,UserRequest request)
    {
        var operation = new UpdateUserCommand(id,request);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<UserResponse>>> GetAllUser()
    {
        var operation = new GetAllUsersQuery();
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpGet("Id")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<UserResponse>> GetByIdUser(int id)
    {
        var operation = new GetUserByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpDelete]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> DeleteUser(int id)
    {
        var operation = new DeleteUserCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}