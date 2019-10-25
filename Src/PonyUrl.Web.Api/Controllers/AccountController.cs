using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PonyUrl.Infrastructure.AspNetCore;
using PonyUrl.Infrastructure.AspNetCore.Authorization;
using PonyUrl.Infrastructure.AspNetCore.Exceptions;
using PonyUrl.Infrastructure.AspNetCore.Models;
using PonyUrl.Infrastructure.AspNetCore.Models.AccountViewModels;
using PonyUrl.Web.Api.Core;

namespace PonyUrl.Web.Api.Controllers
{
    /// <summary>
    /// The controller is manages that user account and login operations.
    /// </summary>
    [AllowAnonymous]
    [ApiExcepitonFilter]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly JwtTokenBuilder _jwtTokenBuilder;

        /// <summary>
        /// C'tor controller
        /// </summary>
        /// <param name="userManager">The manager is manages that user account operations. </param>
        /// <param name="signInManager">The manager is manages that user login operations</param>
        /// <param name="logger">Logger operations</param>
        /// <param name="jwtTokenBuilder"></param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            JwtTokenBuilder jwtTokenBuilder
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtTokenBuilder = jwtTokenBuilder;

        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                throw new ApiException("Login Error");
            }

            var appUser = _userManager.Users.SingleOrDefault(q => q.Email == model.Email);

            var token = await _jwtTokenBuilder.GenerateJwtToken(model.Email, appUser);

            return ResultAt(token);

        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Roles = { AuthConstants.Roles.User },
            };


            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new ApiException("Register Error", 400, result?.Errors?.Select(e => new ErrorModel { Code = e.Code, Description = e.Description }).ToList());
            }

            return ResultAt(new { result = true, message = "The user created successfuly!" }, StatusCodes.Status201Created);
        }
    }
}