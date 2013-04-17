using System;

using Domain.Core;

namespace Domain.Client.Clients
{
    public sealed class LoyaltyCardNumber : IdentityBase<Guid>
    {
        public LoyaltyCardNumber(Guid cardNumber)
        {
            Mandate.ParameterNotDefaut(cardNumber, "cardNumber");

            Id = cardNumber;
        }

        public override string GetTag()
        {
            return "loyaltyCard";
        }

        public override bool IsEmpty()
        {
            return Id == Guid.Empty;
        }
    }
}