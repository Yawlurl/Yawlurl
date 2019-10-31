using MongoDB.Bson.Serialization.Attributes;
using YawlUrl.Common;
using YawlUrl.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Domain
{
    public class Setting : AuditedEntity, IAggregateRoot
    {
        [BsonElement("name")]
        public string Name { get; private set; }

        [BsonElement("value")]
        public dynamic Value { get; private set; }

        public Setting(string name)
        {
            Name = name;
        }

        public void SetValue(dynamic value)
        {
            Check.ArgumentNotNullOrEmpty(value);

            Value = value;
        }
    }
}
