using PonyUrl.Core.Entities;
using System;

namespace PonyUrl.Domain.Common
{
    public abstract class Entity : IEntity<Guid>
    {
        public Guid Id { get; set; }
    } 
}
