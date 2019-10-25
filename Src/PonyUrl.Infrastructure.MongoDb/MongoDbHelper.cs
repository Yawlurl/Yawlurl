using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PonyUrl.Common;

namespace PonyUrl.Infrastructure.MongoDb
{
    public static class MongoDbHelper
    {
        public static string CreateCollectionName(string name)
        {
            Check.ArgumentNotNullOrEmpty(name);

            Check.That<ArgumentOutOfRangeException>(name.Length < 2, "Collection name must be at least 2 characters");

            return $"{char.ToUpper(name[0]) + name.Substring(1)}s";
        }
    }
}
