using AspNetCore.Identity.Mongo.Model;
using PonyUrl.Domain.Entities;

namespace PonyUrl.Infrastructure.AspNetCore.Models
{
    public class ApplicationUser : MongoUser, IUser
    {
        public string UserId => Id;

        public string DisplayName { get; set; }
    }
}
