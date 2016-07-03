using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Popups;
using System.Numerics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Win2DExperimentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const int circleRadius = 12;
        const int circleThickness = 25;
        const int internalWidth = 640;
        const int internalHeight = 480;
        Vector2 spherePosition = new Vector2(0, 120);
        Vector2 sphereSpeed = new Vector2(1, 1) / (TimeSpan.FromSeconds(1.0 / 60).Ticks);
        DateTime startTime;
        bool pausePending = false;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            startTime = DateTime.Now;
        }

        private void canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            var timeSinceLastUpdate = args.Timing.ElapsedTime.Ticks;
            var changeInPosition = sphereSpeed * timeSinceLastUpdate;
            spherePosition = spherePosition + changeInPosition;
            spherePosition.X = spherePosition.X % internalWidth;
            spherePosition.Y = spherePosition.Y % internalHeight;
        }

        private void canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var session = args.DrawingSession;
            var timeElapsed = (DateTime.Now - startTime).TotalSeconds;
            var debug = String.Format("    Time {2}s, Pos {0},{1}, Paused {3}", 
                spherePosition.X, spherePosition.Y, timeElapsed, sender.Paused);
            session.DrawText(debug, new Vector2(0, 0), Colors.Black);
            
            var actualSpherePosition = InternalToActual(sender, spherePosition);
            session.DrawCircle(actualSpherePosition, circleRadius, Colors.Blue, circleThickness);

            if (pausePending)
            {
                pausePending = false;
                sender.Paused = true;
            }
        }

        private void canvas_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
        }

        private void canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var theSender = sender as CanvasAnimatedControl;
            if (theSender.Paused)
            {
                theSender.Paused = false;
            }
            else
            {
                var point = e.GetCurrentPoint(theSender).Position;
                spherePosition = ActualToInternal(theSender, point.ToVector2());
                pausePending = true;
            }
        }

        private Vector2 InternalToActual(ICanvasAnimatedControl map, Vector2 internalCoords)
        {
            // internalCoords * actualSize = actualCoords * internalSize
            return new Vector2(
                (float) map.Size.Width * internalCoords.X / internalWidth,
                (float) map.Size.Height * internalCoords.Y / internalHeight);
        }

        private Vector2 ActualToInternal(ICanvasAnimatedControl map, Vector2 actualCoords)
        {
            return new Vector2(
                actualCoords.X * internalWidth / (float)map.Size.Width,
                actualCoords.Y * internalHeight / (float)map.Size.Height);
        }
    }
}
