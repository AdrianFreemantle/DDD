using System.Linq;

namespace Domain.Core.Infrastructure
{
    public interface IDataQuery
    {
        IQueryable<TPersistable> GetQueryable<TPersistable>() where TPersistable : class;
    }
}