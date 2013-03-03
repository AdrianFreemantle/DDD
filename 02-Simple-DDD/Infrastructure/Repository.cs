using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Infrastructure
{
    /* We want to keep our focus of the details of persisting data at this early stage. Therefore we use 
     * a simple in memory repository to save our data. This has the added advantage in that it introduces
     * the concept of persistence ignorance in a simple manner.
     */
    public interface IGlobalIdentity
    {
        Guid Id { get; }
    }

    public interface IRepository
    {
        IQueryable<TPersistable> GetQuery<TPersistable>() where TPersistable : class, IGlobalIdentity;
        TPersistable Get<TPersistable>(Guid id) where TPersistable : class, IGlobalIdentity;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class, IGlobalIdentity;
        void Remove<TPersistable>(TPersistable item) where TPersistable : class, IGlobalIdentity;
    }

    [DataContract]
    public class InMemoryRepository : IRepository
    {
        [DataMember]
        public Dictionary<Type, object> DataStore { get; set; }
     
        public InMemoryRepository()
        {
            DataStore = new Dictionary<Type, object>();   
        }

        public InMemoryRepository(Dictionary<Type, object> dataStore)
        {
            DataStore = dataStore;
        }

        public TPersistable Get<TPersistable>(Guid id) where TPersistable : class, IGlobalIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            return collection.Single(o => o.Id == id);
        }

        public TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class, IGlobalIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
          
            if(collection.Any(persistable => persistable.Id == item.Id ))
                throw new InvalidOperationException("Item is already in the repository.");   

            collection.Add(item);

            return item;
        }

        public void Remove<TPersistable>(TPersistable item) where TPersistable : class, IGlobalIdentity
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            var existing = Get<TPersistable>(item.Id);
            collection.Remove(existing);
        }

        public IQueryable<TPersistable> GetQuery<TPersistable>() where TPersistable : class, IGlobalIdentity
        {
            return GetCollection<TPersistable>().AsQueryable();
        }

        private HashSet<TPersistable> GetCollection<TPersistable>() where TPersistable : class, IGlobalIdentity
        {
            if (!DataStore.ContainsKey(typeof(TPersistable)))
            {
                var collection = new HashSet<TPersistable>();
                DataStore.Add(typeof(TPersistable), collection);
                return collection;
            }

            return DataStore[typeof(TPersistable)] as HashSet<TPersistable>;
        }
    }
}
