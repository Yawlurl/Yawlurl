using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public  interface IDbSettings
    {
       string ConnectionString { get; set; }
    }
}
