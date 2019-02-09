using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Application.ShortUrls.Queries;

namespace PonyUrl.Web.Api.Controllers
{
    public class ShortUrlController : BaseController
    {

        [HttpGet("ping")]
        public ActionResult Ping()
        {
            return Ok("Pong"); 
        }

        /// <summary>
        /// GetAll Short Url List
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult<ShortUrlListViewModel>> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllShortUrlQuery()));
        }

        // GET api/ShortUrl/DA311F07-E167-43AF-B21F-9A5E7382ED69
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetShortUrlQuery { Id = id }));
        }

        // Post api/ShortUrl/create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlCommand command)
        {
            if (command == null)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        // Post api/ShortUrl/delete
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteShortUrlCommand command)
        {
            if (command == null)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }
    }
}