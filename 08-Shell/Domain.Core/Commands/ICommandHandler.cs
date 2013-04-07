using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Core.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
