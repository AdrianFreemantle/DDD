using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    /* We want to keep our focus of the details of persisting data at this early stage. Therefore we use 
     * a simple in memory repository to save our data. This has the added advantage in that it introduces
     * the concept of persistence ignorance in a simple manner.
     */ 
    
    public interface IIntegerIdentity
    {
        int Id { get; set; }
    }

    public interface IRepository
    {
        IQueryable<TPersistable> GetQuery<TPersistable>() where TPersistable : class, IIntegerIdentity;
        TPersistable Get<TPersistable>(int id) where TPersistable : class, IIntegerIdentity;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity;
        TPersistable Update<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity;
        void Remove<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity;
    }

    public class InMemoryRepository : IRepository
    {
        readonly Dictionary<Type, object> repository = new Dictionary<Type, object>();        

        public TPersistable Get<TPersistable>(int id) where TPersistable : class, IIntegerIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            return collection.Single(o => o.Id == id);
        }

        public TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();

            if(item.Id > 0)
            {
                if(collection.Any(persistable => persistable.Id == item.Id ))
                    throw new InvalidOperationException("Item is already in the repository.");   
            }
            else
            {
                int nextId = 1;

                if (collection.Any())
                    nextId = collection.Last().Id + 1;

                item.Id = nextId;
            }

            collection.Add(item);

            return item;
        }

        public TPersistable Update<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity
        {
            Remove(item);
            return Add(item);
        }

        public void Remove<TPersistable>(TPersistable item) where TPersistable : class, IIntegerIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            var existing = Get<TPersistable>(item.Id);
            collection.Remove(existing);
        }

        public IQueryable<TPersistable> GetQuery<TPersistable>() where TPersistable : class, IIntegerIdentity
        {
            return GetCollection<TPersistable>().AsQueryable();
        }

        private HashSet<TPersistable> GetCollection<TPersistable>() where TPersistable : class, IIntegerIdentity
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
