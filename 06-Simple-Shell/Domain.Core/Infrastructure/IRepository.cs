using System;
using System.Linq;

namespace Domain.Core.Infrastructure
{
    public interface IRepository
    {
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class;
    }
}
