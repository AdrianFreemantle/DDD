using System;
using System.Collections.Generic;
using Domain.Core.Infrastructure;

namespace Infrastructure
{
    public class InMemeoryDocumentStore : IDocumentStore 
    {
        public readonly Dictionary<string, object> Store = new Dictionary<string, object>();

        public TDocument Get<TDocument>(string key) where TDocument : class
        {
            if (!Store.ContainsKey(key))
            {
                throw new KeyNotFoundException(String.Format("No key could be found to match {0}.", key));
            }

            return Store[key] as TDocument;
        }

        public void Save<TDocument>(string key, TDocument document) where TDocument : class
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

        public void Delete(string key)
        {
            if (!Store.ContainsKey(key))
            {
                throw new KeyNotFoundException(String.Format("No key could be found to match {0}.", key));
            }

            Store.Remove(key);
        }
    }
}
