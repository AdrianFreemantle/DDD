using Domain.Core.Infrastructure;

namespace Infrastructure
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly JsonSerializer serializer = new JsonSerializer();
        private byte[] committedData;

        public IRepository Repository { get; set; }

        public InMemoryUnitOfWork()
            :this(new InMemoryRepository())
        {
            
        }

        public InMemoryUnitOfWork(IRepository repository)
        {
            Repository = repository;
            Commit();
        }

        public void Commit()
        {
            //if we were working with a database, this is where the transaction would be created and all changes persisted.
            committedData = serializer.Serialize(Repository);
        }

        public void Rollback()
        {
            Repository = serializer.Deserialize<InMemoryRepository>(committedData);
        }
    }
}