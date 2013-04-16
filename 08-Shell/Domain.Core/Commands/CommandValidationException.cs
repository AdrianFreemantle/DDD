using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Core.Commands
{
    public class CommandValidationException : Exception
    {
        public ReadOnlyCollection<ValidationResult> ValidationResults { get; private set; }

        public CommandValidationException(string message, List<ValidationResult> validationResults)
            : base(message)
        {
            ValidationResults = validationResults.AsReadOnly();
        }

        public CommandValidationException(List<ValidationResult> validationResults)
            : this("A validation exception has occured", validationResults)
        {
        }

        public CommandValidationException(IEnumerable<ValidationResult> validationResults)
            : this("A validation exception has occured", validationResults.ToList())
        {
        }
    }
}
