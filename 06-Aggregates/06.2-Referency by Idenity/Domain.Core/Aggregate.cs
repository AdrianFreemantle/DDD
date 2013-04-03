namespace Domain.Core
{
    public abstract class Aggregate<TIdentity> : Entity<TIdentity> where TIdentity : IHaveIdentity
    {
    }    
}