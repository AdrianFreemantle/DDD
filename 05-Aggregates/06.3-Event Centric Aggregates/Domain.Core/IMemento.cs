namespace Domain.Core
{
    public interface IMemento
    {
        IHaveIdentity Identity { get; set; }
    }
}