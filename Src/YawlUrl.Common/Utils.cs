using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Common
{
    public static class Utils
    {
        public static bool IsEqualUrls(string url1, string url2)
        {
            if (Uri.TryCreate(url1, UriKind.RelativeOrAbsolute, out Uri uri1) && Uri.TryCreate(url2, UriKind.RelativeOrAbsolute, out Uri uri2))
            {
                var result = Uri.Compare(uri1, uri2, UriComponents.Host | UriComponents.PathAndQuery,
                                                     UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                return result == 0;
            }
            else
            {
                return false;
            }
        }
    }
}
