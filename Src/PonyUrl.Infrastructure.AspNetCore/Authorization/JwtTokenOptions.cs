using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.AspNetCore.Authorization
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore { get; set; } = DateTime.UtcNow;

        public DateTime IssueAt { get; set; } = DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(AuthContstants.TokenHoursCount);

        public DateTime Expiration => IssueAt.Add(ValidFor);

        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }

}
