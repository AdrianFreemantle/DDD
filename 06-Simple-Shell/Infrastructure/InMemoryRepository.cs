using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Core.Infrastructure;

namespace Infrastructure
{
    public class InMemoryRepository : IRepository
    {
        readonly Dictionary<Type, object> repository = new Dictionary<Type, object>();

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return GetCollection<TEntity>().AsQueryable();
        }

        public TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            collection.Add(item);
            return item;
        }

        public TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();

            collection.Remove(item);
            return item;
        }

        private HashSet<TPersistable> GetCollection<TPersistable>() where TPersistable : class
        {
            if (!repository.ContainsKey(typeof(TPersistable)))
            {
                var collection = new HashSet<TPersistable>();
                repository.Add(typeof(TPersistable), collection);
                return collection;
            }

            return repository[typeof(TPersistable)] as HashSet<TPersistable>;
        }
    }
}
