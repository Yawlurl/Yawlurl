using System;

namespace PonyUrl.Core
{
    public interface IUserAudit
    {
        Guid CreatorId { get; set; }

        Guid ModifierId { get; set; }
    }
}
