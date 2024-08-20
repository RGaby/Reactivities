using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected IMediator Mediator => _mediator ?? HttpContext.RequestServices.GetService<IMediator>();
        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSucces && result.Value != null)
            {
                return Ok(result.Value);
            }
            if (result.IsSucces && result.Value == null)
            {
                return NotFound();
            }

            return BadRequest(result.Error);
        }

    }
}