using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelf.Pi.Core;
using Shelf.Pi.Core.Clock;

namespace Shelf.Pi.Emulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILightController
    {

        private const int ledSize = 10;
        private const int spacing = 2;
        private const int pixelsPerSegment = 9;

        private List<Shape> leds = new List<Shape>();

        private Dictionary<int, System.Drawing.Color> pendingUpdates = new Dictionary<int, System.Drawing.Color>();

        enum VerticalRow
        {
            TopDown = 24,
            TopUp = 120,
            BottomDown = 144,
            BottomUp = 242
        }

        enum VerticalColumn
        {
            Column1 = 12,
            Column2 = 132,
            Column3 = 252,
            Column4 = 372,
            Column5 = 492,
            Column6 = 612,
            Column7 = 732
        }

        enum HorizontalRow
        {
            Top = 10,
            Middle = 132,
            Bottom = 254
        }

        enum HorizontalColumn
        {
            Column1Left = 24,
            Column1Right = 120,
            Column2Left = 144,
            Column2Right = 240,


            Column3Left = 264,
            Column3Right = 360,
            Column4Left = 384,
            Column4Right = 480,
            Column5Left = 504,
            Column5Right = 600,
            Column6Left = 624,
            Column6Right = 720,
        }

        enum LightRow
        {
            Top = 64,
            Bottom = 186
        }
        enum LightColumn
        {
            Column1 = 64,
            Column2 = 186,
            Column3 = 308,
            Column4 = 430,
            Column5 = 552,
            Column6 = 674
        }

        public MainWindow()
        {
            InitializeComponent();

            // 1st Power Group
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column1, VerticalRow.TopUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column1Left, 1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column1)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column2, VerticalRow.TopDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column1Right, -1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column1)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column1, VerticalRow.BottomDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column1Left, 1));
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column2, VerticalRow.BottomUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column2Left, 1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column2)); 

            // 2nd Power Group
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column3, VerticalRow.TopUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column3Left, 1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column3)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column4, VerticalRow.TopDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column3Right, -1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column3)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column3, VerticalRow.BottomDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column3Left, 1));
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column4, VerticalRow.BottomUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column4Left, 1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column4)); 


            // 3rd Power Group
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column5, VerticalRow.TopUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column5Left, 1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column5)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column6, VerticalRow.TopDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column5Right, -1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column5)); 
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column5, VerticalRow.BottomDown, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column5Left, 1));
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column6, VerticalRow.BottomUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Middle, HorizontalColumn.Column6Left, 1));
            this.leds.Add(CreateLight(LightRow.Bottom, LightColumn.Column6)); 

            // 4th Power Group
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column7, VerticalRow.TopUp, -1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column6Right, -1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column6)); 

            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column4Right, -1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column4)); 

            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Top, HorizontalColumn.Column2Right, -1));
            this.leds.Add(CreateLight(LightRow.Top, LightColumn.Column2)); 

            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column2Left, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column4Left, 1));
            this.leds.AddRange(CreateHorizontalSegment(HorizontalRow.Bottom, HorizontalColumn.Column6Left, 1));
            this.leds.AddRange(CreateVerticalSegment(VerticalColumn.Column7, VerticalRow.BottomUp, -1));


            foreach (var s in this.leds)
            {
                this.drawingCanvas.Children.Add(s);
            }

            

            Task.Run(async () => {
                var host = Daemon.CreateHost()
                    .ConfigureServices((_, services) => {
                        services.AddSingleton<ILightController>(this);
                    })
                    .Build();
                    
                await host.RunAsync();
            });
        }

        private List<Shape> CreateVerticalSegment(VerticalColumn column, VerticalRow row, int direction)
        {
            var segment = new List<Ellipse>();

            for (int i = 0; i < pixelsPerSegment; i++)
            {
                var ellipse = new Ellipse()
                {
                    Width = ledSize,
                    Height = ledSize,
                    Fill = Brushes.Black
                };
                ellipse.SetValue(Canvas.RightProperty, (double)column);
                ellipse.SetValue(Canvas.TopProperty, (double)row + direction * (i * (ledSize + spacing)));
                segment.Add(ellipse);
            }


            return segment.Cast<Shape>().ToList();
        }
        private List<Shape> CreateHorizontalSegment(HorizontalRow row, HorizontalColumn column, int direction)
        {
            var segment = new List<Ellipse>();
            for (int i = 0; i < pixelsPerSegment; i++)
            {
                var ellipse = new Ellipse()
                {
                    Width = ledSize,
                    Height = ledSize,
                    Fill = Brushes.Black
                };
                ellipse.SetValue(Canvas.RightProperty, (double)column + direction * (i * (ledSize + spacing)));
                ellipse.SetValue(Canvas.TopProperty, (double)row);
                segment.Add(ellipse);
            }

            return segment.Cast<Shape>().ToList();
        }

        private Shape CreateLight(LightRow row, LightColumn column)
        {
            var ellipse = new Rectangle()
            {
                Width = ledSize * 2,
                Height = ledSize * 2,
                Fill = Brushes.Black
            };
            ellipse.SetValue(Canvas.RightProperty, (double)column);
            ellipse.SetValue(Canvas.TopProperty, (double)row);
            return ellipse;
        }

        public void SetPixel(int index, System.Drawing.Color color)
        {
            this.pendingUpdates[index] = color;
        }

        public void Update()
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (var update in this.pendingUpdates)
                {
                    this.leds[update.Key].Fill = new SolidColorBrush(Color.FromArgb(update.Value.A, update.Value.R, update.Value.G, update.Value.B));
                }
                this.pendingUpdates.Clear();
            });
        }

        public void Clear()
        {
            for (int i = 0 ; i < 300; i++) 
            {
                this.SetPixel(i, System.Drawing.Color.Black);
                this.Update();
            }
        }
    }
}
