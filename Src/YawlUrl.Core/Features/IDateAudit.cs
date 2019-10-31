using System;

namespace YawlUrl.Core
{
    public interface IDateAudit
    {
        DateTime CreatedDate { get; set; }

        DateTime UpdatedDate { get; set; }
    }
}
