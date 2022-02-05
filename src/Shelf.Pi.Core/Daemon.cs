using Microsoft.Extensions.Hosting;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Core;

public static class Daemon
{
    public static IHostBuilder CreateHost(params string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ClockController>();
                services.AddHostedService<DisplayService>();
            });
    }
}