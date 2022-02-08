using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Core;

public static class Daemon
{
    public static IHostBuilder CreateHost(params string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapPost("/stopAnimations",(AnimationController animationController) => animationController.StopAllAnimations());
                        endpoints.MapPost("/startClock",(AnimationController animationController) => animationController.StartClock());
                    });
                });                
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ClockAnimation>();
                services.AddSingleton<AnimationController>();
                services.AddHostedService<DisplayService>();
            });
    }
}

