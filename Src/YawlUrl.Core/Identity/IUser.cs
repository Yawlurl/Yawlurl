using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public interface IUser
    {
        string UserId { get; }

        string UserName { get; }

        string DisplayName { get; }

        string Email { get; }

        List<string> Roles { get; }

        bool IsAdmin();
    }

    
}
