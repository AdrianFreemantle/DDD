using System.Collections.Generic;
using Domain.Client.Clients;
using System.ComponentModel.DataAnnotations;

namespace Domain.Client.Services
{
    interface IAccountService
    {
        IEnumerable<ValidationResult> CanClientOpenAccount(ClientId clientId);
    }
}
