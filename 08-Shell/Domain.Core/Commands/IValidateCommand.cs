using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Domain.Core.Commands
{
    public interface IValidateCommand<in TCommand> where TCommand : ICommand
    {
        IEnumerable<ValidationResult> Validate(TCommand command);
    }
}