using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Core
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {
    }
}
