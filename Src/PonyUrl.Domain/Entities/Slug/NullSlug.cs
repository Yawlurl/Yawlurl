using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Domain
{
    public class NullSlug : Slug
    {
        public override string Key => string.Empty;
    }
}
