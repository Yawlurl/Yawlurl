using YawlUrl.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Infrastructure.AspNetCore
{
    public class GlobalSettings : IGlobalSettings
    {
        public string RouterDomain { get; set; }
    }
}
