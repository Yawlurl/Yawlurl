using PonyUrl.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore
{
    public class GlobalSettings : IGlobalSettings
    {
        public string RouterDomain { get; set; }
    }
}
