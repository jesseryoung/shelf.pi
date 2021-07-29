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
            
            foreach(var light in Light.Lights) {
                light.SetColor(this.lightController, Color.White.WithBrightness(.25));
            }
            var colorTransition = ColorTransition.GetRandomTranstion(60);
            
            while (false == token.IsCancellationRequested) 
            {
                if (colorTransition.Complete) {
                    colorTransition = ColorTransition.GetRandomTranstion(60, colorTransition.EndColor);
                }
                var color = colorTransition.GetNextColor().WithBrightness(.25);

                var now = DateTime.Now;                
                var hour = now.Hour > 12 ? now.Hour - 12 : now.Hour;
                if (hour > 9)
                {
                    Digit.Digit1.Show(this.lightController, 1, color, true);
                }
                else 
                {
                    Digit.Digit1.Clear(this.lightController);
                }
                Digit.Digit2.Show(this.lightController, (byte)(hour % 10), color);
                Digit.Digit4.Show(this.lightController, (byte)(now.Minute / 10), color);
                Digit.Digit6.Show(this.lightController, (byte)(now.Minute % 10), color);
                this.lightController.Update();
                Thread.Sleep(500);
            }
        }
    }
}