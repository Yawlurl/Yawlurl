using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PonyUrl.Infrastructure.AspNetCore;
using PonyUrl.Web.Api.Core;

namespace PonyUrl.Web.Api.Controllers
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