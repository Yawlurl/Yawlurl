using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PonyUrl.Infrastructure.MongoDb.Identity.Authorization;
using PonyUrl.Infrastructure.MongoDb.Identity.Models;
using PonyUrl.Infrastructure.MongoDb.Identity.Models.AccountViewModels;

namespace PonyUrl.Web.Api.Controllers
{

    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            try
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(q => q.Email == model.Email);

                    var token = await JwtTokenBuilder.GenerateJwtToken(model.Email, appUser, _configuration);
                    return Ok(token);
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException("LOGIN_ERROR");
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            try
            {

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    var token = await JwtTokenBuilder.GenerateJwtToken(model.Email, user, _configuration);
                    return Ok(token);

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("REGISTER_ERROR");
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }
    }
}