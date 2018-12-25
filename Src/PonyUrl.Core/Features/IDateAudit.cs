using System;

namespace PonyUrl.Core
{
    public interface IDateAudit
    {
        DateTime CreatedDate { get; set; }

        DateTime UpdatedDate { get; set; }
    }
}
