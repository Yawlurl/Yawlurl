using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Common;
using YawlUrl.Core;
using YawlUrl.Domain.Core;
using YawlUrl.Infrastructure.AspNetCore;
using YawlUrl.Infrastructure.AspNetCore.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Application
{
    public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser currentUser;
        #endregion

        #region C'tor
        public BaseHandler(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }
        #endregion

        #region Properties
        /// <summary>
        /// User Manager
        /// </summary>
        public UserManager<ApplicationUser> UserManager { get { return _userManager; } }
        /// <summary>
        /// HttpContextAccessor
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        /// <summary>
        /// Current Authenticated User
        /// </summary>
        public virtual ApplicationUser CurrentUser
        {
            get
            {

                return currentUser ?? _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name).GetAwaiter().GetResult();
            }
            set
            {
                currentUser = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual IValidator Validator { get; set; }
        #endregion

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        public virtual void ImpersonateUser(ApplicationUser user)
        {
            CurrentUser = user;
        }

        public virtual void ValidateRequest(TRequest request)
        {
            Check.ArgumentNotNull(Validator);

            var result = Validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ApplicationException(result.Errors);
            }
        }




    }
}
