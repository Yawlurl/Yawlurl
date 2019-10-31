using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YawlUrl.Application
{
    /// <summary>
    /// The exception that is thrown when there is an undesirable situaion in application layer.
    /// </summary>
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {

        }

        public ApplicationException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public ApplicationException(IList<ValidationFailure> failures) : this(string.Join(',', failures))
        {

        }
    }
}
