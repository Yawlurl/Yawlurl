using System;

namespace YawlUrl.Core
{
    public interface IUserAudit
    {
        Guid CreatorId { get; set; }

        Guid ModifierId { get; set; }
    }
}
