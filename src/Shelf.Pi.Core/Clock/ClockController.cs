using System;
using System.Drawing;
using System.Threading;

namespace Shelf.Pi.Core.Clock
{
    public class ClockController
    {

        private readonly ILightController lightController;
        private Random random = new Random();

        public ClockController(ILightController lightController)
        {
            this.lightController = lightController;
        }


        public void Run(CancellationToken token)
        {

            // Set each light box to white
            var boxColor = Color.White.WithBrightness(.25);
            foreach (var light in Light.Lights)
            {
                light.SetColor(this.lightController, boxColor);
            }

            // Create a color transition that should complete ~2x a second
            var transitionFrames = 60;
            var timeBetweenFrames = 500;
            var colorTransition = ColorTransition.GetRandomTranstion(transitionFrames);

            // Loop until told to cancel
            while (false == token.IsCancellationRequested)
            {
                // Create a new transtion if the old one is complete
                if (colorTransition.Complete)
                {
                    colorTransition = ColorTransition.GetRandomTranstion(transitionFrames, colorTransition.EndColor);
                }
                var color = colorTransition.GetNextColor().WithBrightness(.25);
                var now = DateTime.Now;

                // Convert hour to 12 hour clock time
                var hour = ((now.Hour + 11) % 12) + 1;
                if (hour > 9)
                {
                    // Since the first digit is shared with digit 2, flip the "1" so it shows 
                    // on the left side of the segment instead of the right side
                    Digit.Digit1.Show(this.lightController, 1, color, true);
                }
                else
                {
                    // Prior to 10:00 the first digit is empty
                    Digit.Digit1.Clear(this.lightController);
                }
                Digit.Digit2.Show(this.lightController, (byte)(hour % 10), color);
                Digit.Digit4.Show(this.lightController, (byte)(now.Minute / 10), color);
                Digit.Digit6.Show(this.lightController, (byte)(now.Minute % 10), color);
                this.lightController.Update();

                Thread.Sleep(timeBetweenFrames);
            }
        }
    }
}