using AspNetCore.Identity.Mongo.Model;
using PonyUrl.Core;
using System.Linq;

namespace PonyUrl.Infrastructure.AspNetCore.Models
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
