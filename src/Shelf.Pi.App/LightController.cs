using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ws28xx;
using Microsoft.Extensions.Logging;
using Shelf.Pi.Core;

namespace Shelf.Pi.App;

public class LightController : ILightController, IDisposable
{
    private readonly SpiDevice spi;
    private readonly Ws2812b lightStrip;
    private readonly ILogger<LightController> logger;
    private bool disposedValue;

    public LightController(ILogger<LightController> logger)
    {
        this.logger = logger;

        // Use SPI to emulate the "PWM Protocol" that NeoPixels accept.
        var settings = new SpiConnectionSettings(0, 0)
        {
            ClockFrequency = 2_400_000,
            Mode = SpiMode.Mode0,
            DataBitLength = 8
        };

        this.spi = SpiDevice.Create(settings);

        // Use Microsoft's IOT lib to controll the lights
        // The shelf uses 300 Ws2812b (neopixel) lights
        this.lightStrip = new Ws2812b(this.spi, 300);
        this.Clear();
        
    }

    public void SetPixel(int index, Color color)
    {
        // The interface that the IOT lib uses to set light color is a BitmapImage
        // By default this image is 1 pixel high and 300 pixels wide
        this.lightStrip.Image.SetPixel(index, 0, color);
    }

    public void Update()
    {
        this.lightStrip.Update();
    }

    public void Clear()
    {
        this.lightStrip.Image.Clear();
        this.Update();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.Clear();
                this.spi.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}