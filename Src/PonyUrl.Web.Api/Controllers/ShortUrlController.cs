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
using PonyUrl.Web.Api.Core;

namespace PonyUrl.Web.Api.Controllers
{
    /// <summary>
    /// All ShortUrl operations
    /// </summary>    
    [ApiExcepitonFilter]
    public class ShortUrlController : BaseController
    {
        /// <summary>
        /// GetAll Short Url List
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [PonyAuthorize]
        public async Task<IActionResult> GetAll(int? index, int? limit)
        {
            return ResultAt(await Mediator.Send(new GetAllShortUrlQuery(index, limit)));
        }


        /// <summary>
        /// GET api/ShortUrl/PdsaD234
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{slug}")]
        [PonyAuthorize]
        public async Task<IActionResult> Get(string slug, bool boost)
        {
            if (Check.IsNullOrEmpty(slug))
                return BadRequest();

            return ResultAt(await Mediator.Send(new GetShortUrlQuery() { SlugKey = slug, Boost = boost }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [PonyAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlCommand command)
        {
            if (Check.IsNull(command) || Check.IsNullOrEmpty(command.LongUrl))
                return BadRequest();

            return ResultAt(await Mediator.Send(command), StatusCodes.Status201Created);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("bulk-create")]
        [PonyAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateShortUrlCommand command)
        {
            if (Check.IsNull(command))
                return BadRequest();

            return ResultAt(await Mediator.Send(command));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [PonyAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteShortUrlCommand command)
        {
            if (Check.IsNull(command) || Check.IsNullOrEmpty(command.SlugKey))
                return BadRequest();

            return ResultAt(await Mediator.Send(command));
        }
    }
}