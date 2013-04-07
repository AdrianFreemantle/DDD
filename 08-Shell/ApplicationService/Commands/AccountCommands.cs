using Domain.Client.Accounts;
using Domain.Client.ValueObjects;
using Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Commands
{
    public class CancelAccount : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
    }

    public class RegisterMissedPayment : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
    }

    public class RegisterSuccessfullPayment : ICommand
    {
        public AccountNumber AccountNumber { get; set; }
        public BillingResult BillingResult { get; set; }
    }
}
