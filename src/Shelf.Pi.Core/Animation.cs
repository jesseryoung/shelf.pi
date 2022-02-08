namespace Shelf.Pi.Core;


public abstract class Animation
{

    public virtual void Setup(ILightController lightController)
    {

    }

    public abstract bool AnimateFrame(ILightController lightController);


    public virtual void Cleanup(ILightController lightController)
    {
        lightController.Clear();
    }

    public virtual int FrameDelayMilleseconds => 500;
}