using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Domain.Client.Accounts;
using Domain.Client.Clients;
using Domain.Client.Clients.Commands;
using Domain.Core.Commands;

namespace Domain.Client.Validators
{
    public sealed class IssueLoyaltyCardValidator : IValidateCommand<IssueLoyaltyCard>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IClientRepository clientRepository;

        public IssueLoyaltyCardValidator(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            this.accountRepository = accountRepository;
            this.clientRepository = clientRepository;
        }

        public IEnumerable<ValidationResult> Validate(IssueLoyaltyCard command)
        {
            Clients.Client client = clientRepository.Get(command.ClientId);

            if (client.AccountNumber.IsEmpty())
            {
                return new[]{ new ValidationResult("A client must have an account in order to qualify for a loyalty card.") };
            }

            Account account = accountRepository.Get(client.AccountNumber);

            if (!account.QaulifiesForLoyaltyCard())
            {
                return new[] { new ValidationResult("The client's account does not qualify for a loyalty card.") };
            }

            return new ValidationResult[0];
        }
    }
}