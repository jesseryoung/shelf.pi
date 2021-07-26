using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Shelf.Pi.Core {
    public class LightTail: Animation {

        private const double MAX_BRIGHTNESS = 1;
        private const int TARGET_FPS = 100;
        private static readonly Color[] colors = new[] {
            Color.FromArgb(255, 0, 0),
            // Color.FromArgb(255, 165, 0),
            // Color.FromArgb(255, 255, 0),
            // Color.FromArgb(0, 128, 0),
            // Color.FromArgb(0, 0, 255),
            // Color.FromArgb(75, 0, 130),
            // Color.FromArgb(238, 130, 238),
        };
        private readonly Random random = new Random();

        record Pixel(int Index, Color BaseColor);
        private Pixel currentPixel;
        private const int TOTALPIXELS = 300;
        private int direction = 1;
        private Stack<Pixel> pixelHistory = new Stack<Pixel>();

        public LightTail(ILightController lightController)
            : base(lightController, TARGET_FPS)
        {
            this.currentPixel = new Pixel(0, LightTail.colors[random.Next(0, colors.Length)]);
            for (int i = 0; i < 5; i++)
            {
                this.pixelHistory.Push(this.currentPixel);
            }
        }

        public override void OnFrame(int frame)
        {
            var completePercent = (double)frame / (double)TARGET_FPS;

            // when completePercent = 0, brightness = 0; completePercent = 1, brightness = 1
            this.LightController.SetPixel(this.currentPixel.Index,this.currentPixel.BaseColor.WithBrightness(completePercent * LightTail.MAX_BRIGHTNESS));
            var pixelIndex = 0;
            foreach(var pixel in this.pixelHistory) {
                var maxBrightness = 1.0 - ((double)pixelIndex / (double)this.pixelHistory.Count);
                var minBrightness = 1.0 - ((double)(pixelIndex + 1) / (double)this.pixelHistory.Count);
                this.LightController.SetPixel(pixel.Index, pixel.BaseColor.WithBrightness((maxBrightness - completePercent * minBrightness) * LightTail.MAX_BRIGHTNESS));
                pixelIndex +=1;
            }
            
            
        }
        public override void Tick()
        {
            // Remove the last pixel that was animated from the history and add the current one
            var removedPixel = this.pixelHistory.Pop();
            this.pixelHistory.Push(this.currentPixel);
            this.LightController.SetPixel(removedPixel.Index, Color.Black);

            // Color should randomly switch after a pixel has "bounced" back
            var nextColor = this.currentPixel.BaseColor;
            if (this.currentPixel.Index + direction < 0
                || this.currentPixel.Index + direction >= TOTALPIXELS)
            {
                direction *= -1;
                nextColor = colors[random.Next(0, colors.Length)];
            }

            this.currentPixel = new Pixel(this.currentPixel.Index + direction, nextColor);
        }
    }
}