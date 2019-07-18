using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class ShortUrlListViewModel
    {
        public IEnumerable<ShortUrlDto> ShortUrls { get; set; }

        public long TotalCount { get; set; }
    }
}
