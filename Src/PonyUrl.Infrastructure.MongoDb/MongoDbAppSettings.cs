using System;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbAppSettings
    {
        public string ConnectionString { get; set; }

        public static DateTime Now => DateTime.UtcNow;

    }
}
