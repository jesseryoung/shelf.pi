using System;
using System.Threading;

namespace Shelf.Pi.Core
{
    public abstract class Animation 
    {
        public Animation(ILightController lightController, int targetFPS)
        {
            this.LightController = lightController;
            this.targetFPS = targetFPS;
        }

        protected readonly ILightController LightController;
        private readonly int targetFPS;

        public abstract void OnFrame(int frame);

        public abstract void Tick();


        public void Animate(CancellationToken cancellationToken) {
            var lastTick = DateTime.Now;
            var interval = new TimeSpan(0, 0, 0, 1);
            while (cancellationToken.IsCancellationRequested == false) {
                var timeDifference = DateTime.Now - lastTick;
                if (timeDifference > interval) {
                    this.Tick();
                    lastTick = DateTime.Now;
                    timeDifference = DateTime.Now - lastTick;
                }
                var currentFrame = (int)(timeDifference.TotalSeconds * targetFPS);
                this.OnFrame(currentFrame);
                this.LightController.Update();
            }
        }
    }
}