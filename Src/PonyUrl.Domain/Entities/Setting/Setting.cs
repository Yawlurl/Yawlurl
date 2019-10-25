using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Common;
using PonyUrl.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Domain
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
