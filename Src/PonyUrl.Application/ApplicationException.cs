using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PonyUrl.Application
{
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
