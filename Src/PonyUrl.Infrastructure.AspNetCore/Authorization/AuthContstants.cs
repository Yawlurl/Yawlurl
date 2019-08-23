using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore.Authorization
{
    public class AuthContstants
    {
        public const int RefreshTokenDaysCount = 3;
        public const int TokenHoursCount = 5;

        public class AuthenticationSchemes
        {
            public const string Bearer = "Bearer";
        }
    }
}
