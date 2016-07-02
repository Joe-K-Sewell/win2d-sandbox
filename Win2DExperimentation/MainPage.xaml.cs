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
        const int internalWidth = 640;
        const int internalHeight = 480;
        Vector2 spherePosition = new Vector2(0, 240);
        Vector2 sphereSpeed = new Vector2(1, 0) / (TimeSpan.FromSeconds(1.0 / 60).Ticks);
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            
        }

        private void canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            var timeSinceLastUpdate = args.Timing.ElapsedTime.Ticks;
            var changeInPosition = sphereSpeed * timeSinceLastUpdate;
            spherePosition = spherePosition + changeInPosition;
            if (spherePosition.X > internalWidth)
            {
                spherePosition.X = 0;
            }
        }

        private void canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var session = args.DrawingSession;
            var debug = String.Format("Pos {0},{1}", spherePosition.X, spherePosition.Y);
            
            session.DrawText(debug, new Vector2(0, 0), Colors.Black);
            
            // Figure out the actual pixels
            var mapWidth = sender.Size.Width;
            var mapHeight = sender.Size.Height;

            var actualSpherePosition = new Vector2(
                (float)mapWidth * spherePosition.X / internalWidth,
                (float)mapHeight * spherePosition.Y / internalHeight);

            session.DrawText("<marquee>", actualSpherePosition, Colors.Blue);
        }

        private void canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void canvas_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
        }
    }
}
