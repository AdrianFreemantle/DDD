namespace Domain.Core.Infrastructure
{
    public interface IDocumentStore
    {
        TDocument Get<TDocument>(string key) where TDocument : class;
        void Save<TDocument>(string key, TDocument document) where TDocument : class;
        void Delete(string key);
    }
}