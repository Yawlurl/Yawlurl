using System;

namespace PonyUrl.Core
{
    public interface IEntity
    {

    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
