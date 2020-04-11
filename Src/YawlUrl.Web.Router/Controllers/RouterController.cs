using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YawlUrl.Common;
using YawlUrl.Web.Router.Core;
using YawlUrl.Web.Router.Models;
using YawlUrl.Application.ShortUrls.Queries;

namespace YawlUrl.Web.Router.Controllers
{
    public class RouterController : BaseController
    {
        private readonly ILogger<RouterController> _logger;

        public RouterController(ILogger<RouterController> logger)
        {
            _logger = logger;
          
        }
        

        public async Task<IActionResult> Index()
        {
            if (HttpContext.TryGetSlug(out string slug))
            {
                try
                {
                    var yawlurl = await Mediator.Send(new GetShortUrlQuery()
                    {
                        SlugKey = slug,
                        Boost = true,
                        IsRouter = true
                    }).ConfigureAwait(false);

                    if (Check.IsNotNull(yawlurl) && Check.IsValidUrl(yawlurl.LongUrl))
                    {
                        return RedirectPermanent(yawlurl.LongUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Router Index Error");
                }

            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("/generate")]
        public async Task<IActionResult> GenerateYawlLink([FromBody] GenerateUrlModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Check.ArgumentNotUrl(model.LongUrl);

            var yawlUrl = await Mediator.Send(model.ToCommandModel()).ConfigureAwait(false);

            return Ok(new { url = yawlUrl.YawlLink });
        }
    }
}
