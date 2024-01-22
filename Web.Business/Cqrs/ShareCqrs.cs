using Base.Response;
using MediatR;
using Web.Schema;

namespace Web.Business.Cqrs;

public record CreateShareCommand(ShareRequest Model) : IRequest<ApiResponse<ShareResponse>>;
public record UpdateShareCommand(int Id,ShareRequest Model) : IRequest<ApiResponse<ShareResponse>>;
public record DeleteShareCommand(int Id) : IRequest<ApiResponse>;

public record GetAllSharesQuery() : IRequest<ApiResponse<List<ShareResponse>>>;
public record GetShareByIdQuery(int Id) : IRequest<ApiResponse<ShareResponse>>;
