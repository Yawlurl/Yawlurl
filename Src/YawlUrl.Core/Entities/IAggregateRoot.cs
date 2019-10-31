using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Core
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {
    }
}
