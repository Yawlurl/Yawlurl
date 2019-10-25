using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace PonyUrl.Infrastructure.AspNetCore
{
    /// <summary>
    /// Custom Authorize Attribute
    /// </summary>
    public class PonyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Default AuthenticationSchemes is Bearer
        /// </summary>
        public PonyAuthorizeAttribute()
        {
            AuthenticationSchemes = AuthConstants.AuthenticationSchemes.Bearer;
        }
    }
}
