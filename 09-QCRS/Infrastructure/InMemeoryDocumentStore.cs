using System;
using System.Collections.Generic;
using Domain.Core.Infrastructure;

namespace Infrastructure
{
    public class InMemeoryDocumentStore : IDocumentStore 
    {
        public readonly Dictionary<Guid, object> Store = new Dictionary<Guid, object>();

        public TDocument Get<TDocument>(Guid key) where TDocument : class
        {
            if (!Store.ContainsKey(key))
            {
                throw new KeyNotFoundException(String.Format("No key could be found to match {0}.", key));
            }

            return Store[key] as TDocument;
        }

        public void Save<TDocument>(Guid key, TDocument document) where TDocument : class
        {
            if (Store.ContainsKey(key))
            {
                Store[key] = document;
            }
            else
            {
                Store.Add(key, document);
            }
        }

        public void Delete(Guid key)
        {
            if (!Store.ContainsKey(key))
            {
                throw new KeyNotFoundException(String.Format("No key could be found to match {0}.", key));
            }

            Store.Remove(key);
        }
    }
}
