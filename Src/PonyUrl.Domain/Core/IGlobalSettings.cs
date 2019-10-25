using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Domain.Core
{
    public interface IGlobalSettings
    {
        string RouterDomain { get; set; }
    }
}
