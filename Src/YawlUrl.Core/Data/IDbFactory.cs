using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public interface IDbFactory<TDbContext> where TDbContext: IDbContext
    {
        TDbContext Create(IDbSettings dbSettings);
    }
}
