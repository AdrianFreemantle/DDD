using System;

namespace Domain.Core.Infrastructure
{
    public interface IDocumentStore
    {
        TDocument Get<TDocument>(Guid key) where TDocument : class;
        void Save<TDocument>(Guid key, TDocument document) where TDocument : class;
        void Delete(Guid key);
    }
}