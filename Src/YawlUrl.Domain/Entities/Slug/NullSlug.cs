using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Domain
{
    public class NullSlug : Slug
    {
        public override string Key => string.Empty;
    }
}
