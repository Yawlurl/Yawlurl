using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Application.ShortUrls.Commands.BulkCreateShortUrl;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Common;
using PonyUrl.Infrastructure.AspNetCore;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace PonyUrl.Web.Api.Controllers
{
    /// <summary>
    /// All ShortUrl operations
    /// </summary>    
    [ApiExcepitonFilter]
    public class ShortUrlController : BaseController
    {
        /// <summary>
        /// Check api status
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new { Result = true, UtcDateTime = DateTime.UtcNow });
        }

        /// <summary>
        /// GetAll Short Url List
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ShortUrlListViewModel>> GetAll(int skip, int limit)
        {
            

            return Ok(await Mediator.Send(new GetAllShortUrlQuery(skip, limit)));
        }


        /// <summary>
        /// GET api/ShortUrl/DA311F07-E167-43AF-B21F-9A5E7382ED69
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(string key)
        {
            if (Check.IsNullOrEmpty(key))
                return BadRequest();

            var user = HttpContext.User;

            var claimsCount = user.Claims.Count();

            var email = user.HasClaim(h => h.Type == JwtRegisteredClaimNames.Email) ? 
                user.Claims.FirstOrDefault(h => h.Type == JwtRegisteredClaimNames.Email).Value : "";

            return Ok(await Mediator.Send(new GetShortUrlQuery { ShortKey = key }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlCommand command)
        {
            if (Check.IsNull(command) || Check.IsNullOrEmpty(command.LongUrl))
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("bulk-create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateShortUrlCommand command)
        {
            if (Check.IsNull(command))
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteShortUrlCommand command)
        {
            if (Check.IsNull(command) || Check.IsNullOrEmpty(command.ShortKey))
                return BadRequest();
            
            return Ok(await Mediator.Send(command));
        }
    }
}