namespace Domain.Core
{
    public interface IEntity
    {
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
    }
}