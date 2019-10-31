using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public interface IDbManager
    {
        bool ConnectionAvailable(IDbSettings dbSettings);
    }
}
