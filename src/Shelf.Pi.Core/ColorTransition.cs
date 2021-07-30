using System;
using System.Drawing;
using System.Linq;

namespace Shelf.Pi.Core
{
    public class ColorTransition
    {

        // Standard rainbow colors
        private static readonly Color[] colors = new[] {
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(255, 165, 0),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(0, 128, 0),
            Color.FromArgb(0, 0, 255),
            Color.FromArgb(75, 0, 130),
            Color.FromArgb(238, 130, 238),
        };
        private static readonly Random random = new();

        public Color StartColor { get; }
        public Color EndColor { get; }
        private readonly int transitionFrames;

        private int currentFrame;

        public ColorTransition(Color startColor, Color endColor, int transitionFrames)
        {
            this.StartColor = startColor;
            this.EndColor = endColor;
            this.transitionFrames = transitionFrames;
        }

        public Color GetNextColor()
        {

            var percentageComplete = (double)this.currentFrame / (double)this.transitionFrames;

            // Basic linear transition from one color to another
            // At currentFrame = 0, each component should = StartColor
            // At currentFrame = transitionFrames, each component should 0 EndColor
            var r = (int)(((1.0 - percentageComplete) * this.StartColor.R) + (percentageComplete * this.EndColor.R));
            var g = (int)(((1.0 - percentageComplete) * this.StartColor.G) + (percentageComplete * this.EndColor.G));
            var b = (int)(((1.0 - percentageComplete) * this.StartColor.B) + (percentageComplete * this.EndColor.B));

            this.currentFrame++;
            return Color.FromArgb(r, g, b);
        }

        public bool Complete
        {
            get => this.currentFrame >= this.transitionFrames;
        }


        public static ColorTransition GetRandomTranstion(int transitionFrames, Color? startColor = null)
        {
            // Choose a random color to start if none is provided
            var color = startColor ?? colors[random.Next(0, colors.Length)];
            // Make sure not to choose that as the next end color
            var otherColors = colors.Where(c => c != color).ToArray();
            return new ColorTransition(color, otherColors[random.Next(0, otherColors.Length)], transitionFrames);

        }
    }
}