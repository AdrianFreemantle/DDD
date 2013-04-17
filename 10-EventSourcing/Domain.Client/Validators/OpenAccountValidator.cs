using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Domain.Client.Accounts;
using Domain.Client.Accounts.Commands;
using Domain.Core.Commands;
using Domain.Client.Clients;

namespace Domain.Client.Validators
{
    public sealed class OpenAccountValidator : IValidateCommand<OpenAccount>
    {
        private readonly IClientRepository clientRepository;
        private readonly IAccountNumberService accountNumberService;

        public OpenAccountValidator(IClientRepository clientRepository, IAccountNumberService accountNumberService)
        {
            this.clientRepository = clientRepository;
            this.accountNumberService = accountNumberService;
        }

        public IEnumerable<ValidationResult> Validate(OpenAccount command)
        {
            Clients.Client client = clientRepository.Get(command.ClientId);
            AccountNumber accountNumber = accountNumberService.GetAccountNumberForClient(command.ClientId);

            var results = new List<ValidationResult>();

            if (!accountNumber.IsEmpty())
            {
                results.Add(new ValidationResult("A client may not have more than one account."));
            }

            if (!client.ClientMayOpenAccount())
            {
                results.Add(new ValidationResult("The client does not qualify for an account."));
            }

            return results;
        }
    }
}