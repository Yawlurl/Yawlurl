using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Infrastructure.AspNetCore
{
    public class AuthConstants
    {
        public const int RefreshTokenDaysCount = 3;
        public const int TokenHoursCount = 5;

        public class AuthenticationSchemes
        {
            public const string Bearer = "Bearer";
        }

        public class Roles
        {
            /// <summary>
            /// Full Control
            /// </summary>
            public const string Admin = "admin";

            /// <summary>
            /// Create and View own data
            /// </summary>
            public const string User = "user";
        }
    }
}
