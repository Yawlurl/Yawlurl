using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Domain
{
    public class NullShortUrl : ShortUrl
    {
        public override Guid SlugId => Guid.Empty;
        public override string SlugKey => string.Empty;
        public override string LongUrl => string.Empty;
    }
}
