using System;

namespace YawlUrl.Domain
{
    /// <summary>
    /// The exception that is thrown when there is an undesirable situaion in domain layer.
    /// </summary>
    public class DomainException : Exception
    {

        public DomainException(string message) : base(message)
        {

        }
    }
}
