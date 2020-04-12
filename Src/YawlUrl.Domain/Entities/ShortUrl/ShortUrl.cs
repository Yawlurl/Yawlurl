using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YawlUrl.Common;
using YawlUrl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YawlUrl.Domain
{
    public class ShortUrl : AuditedEntity, IAggregateRoot, IHasTag
    {

        [BsonRepresentation(BsonType.String)]
        public virtual Guid SlugId { get; private set; }

        public virtual string SlugKey { get; private set; }

        public virtual string LongUrl { get; private set; }

        public long Hits { get; private set; }

        public bool IsActive { get; private set; }

        public List<string> Tags { get; private set; }

        public ShortUrl(string longUrl = "")
        {
            Check.That<DomainException>(Check.IsNotNullOrEmpty(longUrl) && !Check.IsValidUrl(longUrl), "Url is not valid!");
            Tags = new List<string>();
            LongUrl = longUrl;
        }
        /// <summary>
        /// Increases default one level the short url hits
        /// </summary>
        public void Boost(int value = 1)
        {
            Hits += value;
        }
        /// <summary>
        /// Set 0 the short url hits
        /// </summary>
        public void ResetBoost()
        {
            Hits = 0;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void DeActivate()
        {
            IsActive = false;
        }

        public void SetSlug(Slug slug)
        {
            Check.That<DomainException>(Check.IsNull(slug), "Slug is required");

            SlugId = slug.Id;
            SlugKey = slug.Key;
        }

        public bool IsAnonymous()
        {
            return CreatedBy.Equals(Guid.Empty.ToString());
        }

        #region Tags Methods

        public void AddTag(string tag)
        {
            if(!Tags.Any(t=> t.Equals(tag)))
            {
                Tags.Add(tag.ToLower());
            }
        }

        public void RemoveTag(string tag)
        {
            if (Tags.Any(t => t.Equals(tag.ToLower())))
            {
                Tags.Remove(tag.ToLower());
            }
        }

        public void ClearTags()
        {
            Tags.Clear();
        }

        #endregion

    }
}
