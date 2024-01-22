using Base.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Business.Cqrs;
using Web.Schema;

namespace Exchange_Trade_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SharesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ShareResponse>> CreateShare(ShareRequest request)
        {
            var command = new CreateShareCommand(request);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ShareResponse>> UpdateShare(int id, ShareRequest request)
        {
            var command = new UpdateShareCommand(id, request);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteShare(int id)
        {
            var command = new DeleteShareCommand(id);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<List<ShareResponse>>> GetAllShares()
        {
            var query = new GetAllSharesQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<ShareResponse>> GetShareById(int id)
        {
            var query = new GetShareByIdQuery(id);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}