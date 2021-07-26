namespace System.Drawing
{
    public static class DrawingExtensions
    {
        public static Color WithBrightness(this Color color, double brightness) =>
            Color.FromArgb((int)(color.R * brightness), (int)(color.G * brightness), (int)(color.B * brightness));
    }
}