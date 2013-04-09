namespace Domain.Core.Commands
{
    public interface ICommandPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : ICommand;
    }
}
