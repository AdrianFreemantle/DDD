namespace Domain.Core
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T subject);
    }
}
