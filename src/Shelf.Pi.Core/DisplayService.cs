using Microsoft.Extensions.Hosting;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Core;


public class DisplayService : IHostedService
{
    private readonly ClockController clockController;

    public DisplayService(ClockController clockController)
    {
        this.clockController = clockController;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.clockController.Run(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}