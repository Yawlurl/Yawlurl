using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Common;
using PonyUrl.Core;


namespace PonyUrl.Domain
{
    public class Slug : AuditedEntity, IAggregateRoot
    {
        public virtual string Key { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsRandom { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="isRandom"></param>
        public Slug(string key = "", bool isRandom = true)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            IsRandom = isRandom;
            Key = isRandom ? key.Trim() : key.Trim().ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRandom"></param>
        public void SetKey(string key, bool isRandom)
        {
            Check.That<DomainException>(string.IsNullOrWhiteSpace(key), $"Key is required");
            IsRandom = isRandom;
            Key = isRandom ? key.Trim() : key.Trim().ToLower();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void DeActivate()
        {
            IsActive = false;
        }

        public void SetOwner(IUser user)
        {
            Check.That<DomainException>(Check.IsNull(user), "The user is null!");

            CreatedBy = user.UserId;
        }
    }
}
