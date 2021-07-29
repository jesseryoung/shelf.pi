using System.Collections.Generic;
using System.Drawing;

namespace Shelf.Pi.Core.Clock
{
    public class Digit
    {

        private static Dictionary<byte, int[]> digitIndexMap = new()
        {
            { 0, new[] { 0, 1, 2, 3, 4, 5 } },
            { 1, new[] { 1, 2 } },
            { 2, new[] { 0, 1, 3, 4, 6 } },
            { 3, new[] { 0, 1, 2, 3, 6 } },
            { 4, new[] { 1, 2, 5, 6 } },
            { 5, new[] { 0, 2, 3, 5, 6 } },
            { 6, new[] { 0, 2, 3, 4, 5, 6 } },
            { 7, new[] { 0, 1, 2 } },
            { 8, new[] { 0, 1, 2, 3, 4, 5, 6 } },
            { 9, new[] { 0, 1, 2, 3, 5, 6 } },
        };

        private readonly Segment[] segments;


        private Digit(Segment[] segments)
        {
            this.segments = segments;
        }

        public void Show(ILightController lightController, byte number, Color color, bool oneFlipped = false)
        {
            this.Clear(lightController);
            if (oneFlipped && number == 1)
            {
                this.segments[4].SetColor(lightController, color);
                this.segments[5].SetColor(lightController, color);
            }
            else
            {
                foreach (var index in digitIndexMap[number])
                {
                    this.segments[index].SetColor(lightController, color);
                }
            }

        }

        public void Clear(ILightController lightController)
        {
            foreach (var segment in this.segments)
            {
                segment.SetColor(lightController, Color.Black);
            }
        }


        public static readonly Digit Digit1 = new Digit(new[] {
            Segment.Segments[25],
            Segment.Segments[18],
            Segment.Segments[22],
            Segment.Segments[30],
            Segment.Segments[31],
            Segment.Segments[24],
            Segment.Segments[23],
        });

        public static readonly Digit Digit2 = new Digit(new[] {
            Segment.Segments[17],
            Segment.Segments[16],
            Segment.Segments[20],
            Segment.Segments[21],
            Segment.Segments[22],
            Segment.Segments[18],
            Segment.Segments[19],
        });

        public static readonly Digit Digit3 = new Digit(new[] {
            Segment.Segments[26],
            Segment.Segments[10],
            Segment.Segments[14],
            Segment.Segments[29],
            Segment.Segments[20],
            Segment.Segments[16],
            Segment.Segments[15],
        });

        public static readonly Digit Digit4 = new Digit(new[] {
            Segment.Segments[9],
            Segment.Segments[8],
            Segment.Segments[12],
            Segment.Segments[13],
            Segment.Segments[14],
            Segment.Segments[10],
            Segment.Segments[11],
        });
        public static readonly Digit Digit5 = new Digit(new[] {
            Segment.Segments[27],
            Segment.Segments[2],
            Segment.Segments[6],
            Segment.Segments[28],
            Segment.Segments[12],
            Segment.Segments[8],
            Segment.Segments[7],
        });
        public static readonly Digit Digit6 = new Digit(new[] {
            Segment.Segments[1],
            Segment.Segments[0],
            Segment.Segments[4],
            Segment.Segments[5],
            Segment.Segments[6],
            Segment.Segments[2],
            Segment.Segments[3],
        });
    }
}
