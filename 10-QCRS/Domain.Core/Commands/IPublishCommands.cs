namespace Domain.Core.Commands
{
    public interface IPublishCommands
    {
        void Publish<TEvent>(TEvent @event) where TEvent : ICommand;
    }
}
