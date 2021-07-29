using System.Drawing;

namespace Shelf.Pi.Core.Clock
{
    public class Light
    {
        public static int[] LightIndexes = new[] {
            18,
            37,
            74,
            93,
            112,
            149,
            168,
            187,
            224,
            243,
            253,
            263,
        };

        private readonly int index;

        private Light(int index)
        {
            this.index = index;
        }

        public void SetColor(ILightController lightController, Color color)
        {
            lightController.SetPixel(this.index, color);
        }

        public static Light[] Lights;

        static Light()
        {
            Light.Lights = new Light[Light.LightIndexes.Length];
            for (int i = 0; i < Light.LightIndexes.Length; i++)
            {
                Light.Lights[i] = new Light(Light.LightIndexes[i]);
            }
        }
    }
}
