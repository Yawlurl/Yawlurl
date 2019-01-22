using AspNetCore.Identity.Mongo.Model;
using PonyUrl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.MongoDb.Identity.Models
{
    public class ApplicationUser : MongoUser, IUser
    {
        public string UserId => Id;

        public string DisplayName { get; set; }

       
    }
}
