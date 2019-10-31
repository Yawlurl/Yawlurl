using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YawlUrl.Infrastructure.AspNetCore;
using YawlUrl.Web.Api.Core;

namespace YawlUrl.Web.Api.Controllers
{
    public class HealthCheckController : BaseController
    {
        /// <summary>
        /// Check api status
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        [AllowAnonymous]
        [ApiExcepitonFilter]
        public IActionResult Ping()
        {
            return ResultAt(new { Result = true, DateTimeUtc = DateTime.UtcNow });
        }
    }
}