using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.MongoDb
{
    public static class MongoDbHelper
    {
        public static string CreateCollectionName(string name)
        {
            return name.ToLowerInvariant() + "_list";
        }
    }
}
