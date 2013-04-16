using System;
using System.Collections.Generic;

using Domain.Client.Accounts;
using Domain.Client.Clients;
using System.ComponentModel.DataAnnotations;

using Domain.Core.Infrastructure;
using Domain.Core;

namespace Domain.Client.Services
{
    public interface IAccountNumberService
    {
        AccountNumber GetNextAccountNumber();
    }
}
