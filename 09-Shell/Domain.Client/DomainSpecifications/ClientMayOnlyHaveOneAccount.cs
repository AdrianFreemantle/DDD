using Domain.Client.Accounts.Commands;
using Domain.Core.Commands;

namespace Domain.Client.DomainSpecifications
{
    public abstract class ClientMayOnlyHaveOneAccountRule : ICommandSpecification<OpenAccount>
    {
        public string ErrorMessage { get { return "A client may not have more than one account."; } }
        public abstract bool IsValid(OpenAccount command);
    }
}