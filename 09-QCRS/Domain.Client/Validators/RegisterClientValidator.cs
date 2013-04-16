using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Domain.Client.Clients;
using Domain.Client.Clients.Commands;
using Domain.Core.Commands;
using Domain.Client.ValueObjects;

namespace Domain.Client.Validators
{
    public sealed class RegisterClientValidator : IValidateCommand<RegisterClient>
    {
        private readonly IClientRepository clientRepository;

        public RegisterClientValidator(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public IEnumerable<ValidationResult> Validate(RegisterClient command)
        {
            if (ClientIsRegistered(command.IdentityNumber))
            {
                return new[] { new ValidationResult("The client is already registered.") };
            }

            return new ValidationResult[0];
        }

        private bool ClientIsRegistered(IdentityNumber identityNumber)
        {
            try
            {
                return clientRepository.Get(identityNumber) != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
