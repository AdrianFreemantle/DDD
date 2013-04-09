using System;

namespace Domain.Core.Infrastructure
{
    public interface IRepository
    {
        TPersistable Get<TPersistable>(object id) where TPersistable : class;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class;
    }
}
