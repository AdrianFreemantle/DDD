namespace Domain.Core.Commands
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
