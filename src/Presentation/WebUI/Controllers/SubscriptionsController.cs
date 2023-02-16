using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Subscriptions.Commands.SubscribeToSearch;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class SubscriptionsController : BaseController
{
    public async Task<IActionResult> SearchAsync(SubscribeToSearchCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }

    public async Task<IActionResult> Subscribe(SubscribeToSearchCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
}
