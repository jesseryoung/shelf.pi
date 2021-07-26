using System;
using System.Drawing;

namespace Shelf.Pi.Core
{
    public interface ILightController
    {
        void SetPixel(int index, Color color);

        void Update();
    }
}
