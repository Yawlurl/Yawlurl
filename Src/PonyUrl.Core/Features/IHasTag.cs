using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Core
{
    public interface IHasTag
    {
        List<string> Tags { get; }
    }
}
