using Domain.Core;

namespace Tests.ValueObjects
{
    public sealed class ClientId : IdentityBase<string>
    {
        public override string Id { get; protected set; }

        public ClientId(IdentityNumber identityNumber)
        {
            Id = identityNumber.Number;
        }

        public override string GetTag()
        {
            return "client-id";
        }
    }
}