using Domain.Client.Clients.Commands;
using Domain.Core.Commands;

namespace Domain.Client.Clients
{
    public interface IClientApplicationService: 
        IHandleCommand<RegisterClient>,
        IHandleCommand<CorrectDateOfBirth>,
        IHandleCommand<SetClientAsDeceased>
    {
    }
}
