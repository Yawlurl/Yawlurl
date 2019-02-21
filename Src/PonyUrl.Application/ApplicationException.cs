using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PonyUrl.Application
{
    /// <summary>
    /// The exception that is thrown when there is an undesirable situaion in application layer.
    /// </summary>
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {

        }

        public ApplicationException(IList<ValidationFailure> failures) : this(string.Join(',', failures))
        {

        }
    }
}
