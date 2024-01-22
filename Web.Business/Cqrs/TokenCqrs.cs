using Base.Response;
using MediatR;
using Web.Schema;

namespace Web.Business.Cqrs;

public class TokenCqrs
{
    public record CreateTokenCommand(TokenRequest Model) : IRequest<ApiResponse<TokenResponse>>;

}