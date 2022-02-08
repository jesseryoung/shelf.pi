using Microsoft.Extensions.Hosting;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Core;


public class DisplayService : IHostedService
{
    private readonly AnimationController animationController;

    public DisplayService(AnimationController animationController)
    {
        this.animationController = animationController;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await this.animationController.Run(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}