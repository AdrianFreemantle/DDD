namespace Commands
{
    public interface ICommandSpecification<in TCommand> where TCommand : ICommand
    {
        string ErrorMessage { get; }
        bool IsValid(TCommand command);
    }
}
