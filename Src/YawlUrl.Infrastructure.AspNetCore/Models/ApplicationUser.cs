using AspNetCore.Identity.Mongo.Model;
using YawlUrl.Core;
using System.Linq;

namespace YawlUrl.Infrastructure.AspNetCore.Models
{
    public class ApplicationUser : MongoUser, IUser
    {
        public string UserId => Id.Trim();

        public string DisplayName { get; set; }

        public bool IsAdmin()
        {
            return Roles.Any(r => r.Equals(AuthConstants.Roles.Admin));
        }
    }
}
