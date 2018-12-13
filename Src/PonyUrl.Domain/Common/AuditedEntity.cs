using PonyUrl.Domain.Entities.Users;
using System;

namespace PonyUrl.Domain.Common
{
    public abstract class AuditedEntity : Entity
    {
        public AuditedEntity()
        {

        }
         
        public DateTime CreatedDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual User UpdatedBy { get; set; }
    }
}
