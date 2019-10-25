using MediatR;
using PonyUrl.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace PonyUrl.Web.Api.Core
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;

        /// <summary>
        /// Common Mediator
        /// </summary>
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

        public string ConsumerCode
        {
            get
            {
                return HttpContext.Request.GetHeaderValue<string>("consumerCode");
            }
        }

        public string TraceId
        {
            get
            {
                return HttpContext.Request.GetHeaderValue<string>("traceId");
            }
        }

        public IActionResult ResultAt<T>(T data)
        {
            if (Check.IsNull(data)) return NoContent();

            var result = new Output<T>(data).AddConsumerCode(ConsumerCode).AddTraceId(TraceId);
         
            return Ok(result);
        }

        public IActionResult ResultAt<T>(T data, int statusCode)
        {
            if (Check.IsNull(data)) return NoContent();

            var result = new Output<T>(data).AddConsumerCode(ConsumerCode).AddTraceId(TraceId);

            return StatusCode(statusCode, result);
        }
    }
}
