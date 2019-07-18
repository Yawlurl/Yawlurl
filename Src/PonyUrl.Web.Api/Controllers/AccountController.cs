using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PonyUrl.Infrastructure.AspNetCore;
using PonyUrl.Infrastructure.AspNetCore.Authorization;
using PonyUrl.Infrastructure.AspNetCore.Exceptions;
using PonyUrl.Infrastructure.AspNetCore.Models;
using PonyUrl.Infrastructure.AspNetCore.Models.AccountViewModels;

namespace PonyUrl.Web.Api.Controllers
{
    /// <summary>
    /// The controller is manages that user account and login operations.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // private readonly IEmailSender _emailSender;
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ApiExcepitonFilter]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                throw new ApiException("login_error");
            }

            var appUser = _userManager.Users.SingleOrDefault(q => q.Email == model.Email);

            var token = await _jwtTokenBuilder.GenerateJwtToken(model.Email, appUser);

            return Ok(token);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ApiExcepitonFilter]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new ApiException("register_error");
            }

            await _signInManager.SignInAsync(user, false);

            var token = await _jwtTokenBuilder.GenerateJwtToken(model.Email, user);

            return Ok(token);
        }
    }
}