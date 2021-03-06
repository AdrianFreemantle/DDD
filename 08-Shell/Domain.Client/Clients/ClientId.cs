using System;
using Domain.Client.ValueObjects;
using Domain.Core;

namespace Domain.Client.Clients
{
    public sealed class ClientId : IdentityBase<string>
    {
        public override string Id { get; protected set; }

        public ClientId(string clientId)
        {
            Mandate.ParameterNotNullOrEmpty(clientId, "clientId");

            Id = clientId;
        }

        public ClientId(IdentityNumber identityNumber)
        {
            Mandate.ParameterNotDefaut(identityNumber, "identityNumber");

            Id = identityNumber.Number;
        }

        public override string GetTag()
        {
            return "client-id";
        }

        public override bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Id);
        }
    }
}