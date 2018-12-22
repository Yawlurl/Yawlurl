using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Integration.Test.MongoDb
{
    public class TestEntity : AuditedEntity
    {
        [BsonElement("title")]
        public string Title { get; set; }


        public TestEntity()
        {

        }

        public TestEntity(string title)
        {
            Title = title;
        }
    }
}
