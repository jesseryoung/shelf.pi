using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Core;



public class AnimationController : IDisposable
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILightController lightController;
    private readonly ILogger<AnimationController> logger;
    private CancellationTokenSource animationCancellationToken = new();
    private TaskCompletionSource animationStartTask = new();
    private bool disposedValue;

    private readonly object cancelTokenLock = new();

    public AnimationController(IServiceProvider serviceProvider, ILightController lightController, ILogger<AnimationController> logger)
    {
        this.serviceProvider = serviceProvider;
        this.lightController = lightController;
        this.logger = logger;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        // Outer loop. Run until daemon is topped.
        while (false == cancellationToken.IsCancellationRequested)
        {
            // Reset the cts that stopped the current animation
            if (this.animationCancellationToken.IsCancellationRequested)
            {
                lock (this.cancelTokenLock)
                {
                    this.animationCancellationToken.Dispose();
                    this.animationCancellationToken = new();
                }
            }
            using (var scope = serviceProvider.CreateScope())
            {
                var animation = scope.ServiceProvider.GetRequiredService<ClockAnimation>();
                // Make sure RunAnimation stops when the deameon is requested to stop
                using (cancellationToken.Register(() => this.animationCancellationToken.Cancel()))
                {
                    await RunAnimation(animation, this.animationCancellationToken.Token);
                }
            }
            // Only need to wait for the restart if cancel was called, otherwise you can assume that the animation completed
            if (this.animationCancellationToken.IsCancellationRequested)
            {
                // Wait until the deamon is killed or until the clock is requested to start again.
                await this.animationStartTask.Task.OrUntilCanceled(cancellationToken);
            }
        }
    }

    public void StopAllAnimations()
    {
        lock (this.cancelTokenLock)
        {
            this.animationCancellationToken.Cancel();
        }

        this.logger.LogInformation("Stop all animations requested.");
    }

    public void StartClock()
    {
        this.animationStartTask.SetResult();
        this.animationStartTask = new();
        this.logger.LogInformation("Start clock animation requested.");
    }

    private async Task RunAnimation(Animation animation, CancellationToken cancellationToken)
    {
        animation.Setup(this.lightController);
        while (false == cancellationToken.IsCancellationRequested)
        {
            animation.AnimateFrame(this.lightController);
            await Task.Delay(animation.FrameDelayMilleseconds).OrUntilCanceled(cancellationToken);
        }
        animation.Cleanup(this.lightController);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.animationCancellationToken.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}