using System;
using System.Device.Spi;
using System.Drawing;
using System.Threading;
using Iot.Device.Ws28xx;
using Shelf.Pi.Core;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.App
{
    class Program : ILightController
    {
        private readonly Ws2812b lightStrip;

        static void Main(string[] args)
        {
            // Use SPI to emulate the "PWM Protocol" that NeoPixels accept.
            var settings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            using SpiDevice spi = SpiDevice.Create(settings);

            var lightController = new Program(spi);
            var clockController = new ClockController(lightController);

            var cts = new CancellationTokenSource();

            // Notify ClockController when it's time to stop running
            Console.CancelKeyPress += (s,e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            clockController.Run(cts.Token);
            // Clear the lights after exiting. 
            // Lights will stay lit even after the program exits unless told to shut off.
            lightController.Clear();
        }

        public Program(SpiDevice spi)
        {
            // Use Microsoft's IOT lib to controll the lights
            // The shelf uses 300 Ws2812b (neopixel) lights
            this.lightStrip = new Ws2812b(spi, 300);
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
    }
}
