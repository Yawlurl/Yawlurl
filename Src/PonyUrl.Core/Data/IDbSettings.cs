using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Core
{
    public  interface IDbSettings
    {
       string ConnectionString { get; set; }
    }
}
