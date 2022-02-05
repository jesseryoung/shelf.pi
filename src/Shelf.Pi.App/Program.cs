using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelf.Pi.App;
using Shelf.Pi.Core;


var host = Daemon
    .CreateHost(args)
    .ConfigureServices((_, services) => {
        services.AddSingleton<ILightController, LightController>();
    })
    .Build();

await host.RunAsync();