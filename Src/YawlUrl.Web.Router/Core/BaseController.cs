using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace YawlUrl.Web.Router.Core
{
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
        /// <summary>
        /// Common Mediator
        /// </summary>
        public virtual IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}