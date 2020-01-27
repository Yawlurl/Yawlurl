using AspNetCore.Identity.Mongo.Model;
using YawlUrl.Core;
using System.Linq;
using System;

namespace YawlUrl.Infrastructure.AspNetCore.Models
{
    public class ApplicationUser : MongoUser, IUser
    {
        public virtual string UserId => Id.Trim();

        public virtual string DisplayName { get; set; }

        public virtual bool IsAdmin()
        {
            return Roles.Any(r => r.Equals(AuthConstants.Roles.Admin));
        }
    }

    public class AnonymousUser : ApplicationUser
    {
        public override string UserId => Guid.Empty.ToString();

        private static readonly ApplicationUser anonymousUser;

        public static ApplicationUser Current()
        {
            return anonymousUser ?? new ApplicationUser();
        }
    }
}
