using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Core
{
    public interface IDbManager
    {
        bool ConnectionAvailable(IDbSettings dbSettings);
    }
}
