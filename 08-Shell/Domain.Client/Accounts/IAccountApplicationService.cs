using Domain.Client.Accounts.Commands;
using Domain.Core.Commands;

namespace Domain.Client.Accounts
{
    public interface IAccountApplicationService :
        IHandleCommand<CancelAccount>,
        IHandleCommand<OpenAccount>,
        IHandleCommand<RegisterMissedPayment>,
        IHandleCommand<RegisterSuccessfullPayment>
    {
    }
}
