using System;
using System.Drawing;
using System.Threading;

namespace Shelf.Pi.Core.Clock;
public class ClockAnimation : Animation
{
    private const int TransitionFrames = 60;
    public override int FrameDelayMilleseconds => 500;
    private Random random = new Random();
    private ColorTransition colorTransition;

    public ClockAnimation()
    {
        // Create a color transition that should complete ~2x a minute
        this.colorTransition = ColorTransition.GetRandomTranstion(TransitionFrames);
    }

    public override void Setup(ILightController lightController)
    {
        // Set each light box to white
        var boxColor = Color.White.WithBrightness(.25);
        foreach (var light in Light.Lights)
        {
            light.SetColor(lightController, boxColor);
        }
        lightController.Update();
    }

    public override bool AnimateFrame(ILightController lightController)
    {
        // Create a new transtion if the old one is complete
        if (colorTransition.Complete)
        {
            this.colorTransition = ColorTransition.GetRandomTranstion(TransitionFrames, this.colorTransition.EndColor);
        }
        var color = colorTransition.GetNextColor().WithBrightness(.25);
        var now = DateTime.Now;

        // Convert hour to 12 hour clock time
        var hour = ((now.Hour + 11) % 12) + 1;
        if (hour > 9)
        {
            // Since the first digit is shared with digit 2, flip the "1" so it shows 
            // on the left side of the segment instead of the right side
            Digit.Digit1.Show(lightController, 1, color, true);
        }
        else
        {
            // Prior to 10:00 the first digit is empty
            Digit.Digit1.Clear(lightController);
        }
        Digit.Digit2.Show(lightController, (byte)(hour % 10), color);
        Digit.Digit4.Show(lightController, (byte)(now.Minute / 10), color);
        Digit.Digit6.Show(lightController, (byte)(now.Minute % 10), color);
        lightController.Update();

        return true;
    }
}
