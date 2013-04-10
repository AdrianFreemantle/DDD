using Domain.Client.Clients.Commands;
using Domain.Core.Commands;

namespace Domain.Client.DomainSpecifications
{
    public abstract class ClientMayOnlyBeRegisteredOnceRule : ICommandSpecification<RegisterClient>
    {
        public string ErrorMessage { get { return "The client is already registered."; } }
        public abstract bool IsValid(RegisterClient command);
    }
}
