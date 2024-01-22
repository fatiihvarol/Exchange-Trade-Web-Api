
using Base.Response;
using MediatR;
using Web.Schema;

namespace Web.Business.Cqrs
{
    public record CreatePortfolioCommand(PortfolioRequest Model) : IRequest<ApiResponse<PortfolioResponse>>;
    public record UpdatePortfolioCommand(int PortfolioId, PortfolioRequest Model) : IRequest<ApiResponse<PortfolioResponse>>;
    public record DeletePortfolioCommand(int PortfolioId) : IRequest<ApiResponse>;

    public record GetAllPortfoliosQuery() : IRequest<ApiResponse<List<PortfolioResponse>>>;
    public record GetPortfolioByIdQuery(int PortfolioId) : IRequest<ApiResponse<PortfolioResponse>>;
}
