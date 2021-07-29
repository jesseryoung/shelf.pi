using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Shelf.Pi.Core.Clock 
{
    public class Segment
    {
        private readonly int[] lightIndexes;

        private Segment(int[] lightIndexes)
        {
            this.lightIndexes = lightIndexes;
        }

        public void SetColor(ILightController lightController, Color color) 
        {
            foreach(var index in this.lightIndexes)
            {
                lightController.SetPixel(index, color);
            }
        }

        static Segment()
        {
            Segment.Segments = new Segment[32];
            int segmentStart = 0;
            for (int i = 0; i < 32; i++)
            {
                if (Light.LightIndexes.Contains(segmentStart)) {
                    segmentStart += 1;
                }
                Segment.Segments[i] = new Segment(Enumerable.Range(segmentStart, 9).ToArray());
                segmentStart += 9;
            }
        }
        public static readonly Segment[] Segments;
    }

}
