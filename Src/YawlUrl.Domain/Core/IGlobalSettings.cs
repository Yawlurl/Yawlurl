using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Domain.Core
{
    public interface IGlobalSettings
    {
        string RouterDomain { get; set; }
    }
}
