using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public interface IHasTag
    {
        List<string> Tags { get; }
    }
}
