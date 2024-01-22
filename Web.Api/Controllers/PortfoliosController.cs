using Base.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Business.Cqrs;
using Web.Schema;

namespace Exchange_Trade_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PortfoliosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<PortfolioResponse>>> GetAllPortfolios()
        {
            var operation = new GetAllPortfoliosQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<PortfolioResponse>> GetPortfolioById(int id)
        {
            var operation = new GetPortfolioByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<PortfolioResponse>> CreatePortfolio(PortfolioRequest request)
        {
            var operation = new CreatePortfolioCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<PortfolioResponse>> UpdatePortfolio(int id, PortfolioRequest request)
        {
            var operation = new UpdatePortfolioCommand(id, request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeletePortfolio(int id)
        {
            var operation = new DeletePortfolioCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}