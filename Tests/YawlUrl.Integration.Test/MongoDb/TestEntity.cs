using MongoDB.Bson.Serialization.Attributes;
using YawlUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Integration.Test.MongoDb
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
