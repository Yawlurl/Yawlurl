using PonyUrl.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Domain.Entities
{
    public interface IUser
    {
        string UserId { get; }

        string UserName { get; }

        string DisplayName { get; }
    }
}
