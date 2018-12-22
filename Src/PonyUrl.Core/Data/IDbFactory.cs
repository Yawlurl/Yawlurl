using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Core
{
    public interface IDbFactory<TDbContext> where TDbContext: IDbContext
    {
        TDbContext Create(IDbSettings dbSettings);
    }
}
