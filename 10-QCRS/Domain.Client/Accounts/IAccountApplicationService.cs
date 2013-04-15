using Domain.Client.Accounts.Commands;
using Domain.Client.Clients.Events;
using Domain.Core.Commands;
using Domain.Core.Events;

namespace Domain.Client.Accounts
{
    public interface IAccountApplicationService :
        IHandleCommand<CancelAccount>,
        IHandleCommand<OpenAccount>,
        IHandleCommand<RegisterMissedPayment>,
        IHandleCommand<RegisterSuccessfullPayment>,
        IHandleEvent<ClientPassedAway>
    {
    }
}
