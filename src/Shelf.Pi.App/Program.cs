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

            Console.CancelKeyPress += (s,e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            clockController.Run(cts.Token);
            lightController.Clear();
        }

        public Program(SpiDevice spi)
        {
            var pixel_count = 300;
            this.lightStrip = new Ws2812b(spi, pixel_count);
            this.Clear();
        }

        public void SetPixel(int index, Color color)
        {
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
