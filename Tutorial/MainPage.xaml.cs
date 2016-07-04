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
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Tutorial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Random ayn = new Random();
        private Vector2 RndPos()
        {
            return new Vector2(
                (float) ayn.NextDouble() * 500f,
                (float) ayn.NextDouble() * 500f);
        }

        private float RndRad()
        {
            return (float) ayn.NextDouble() * 150f;
        }

        private byte RndByte()
        {
            return (byte) ayn.Next(256);
        }

        private Color RndColor()
        {
            return Color.FromArgb(255, RndByte(), RndByte(), RndByte());
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void myCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //this.myCanvas.RemoveFromVisualTree();
            //this.myCanvas = null;
        }

        GaussianBlurEffect blurEffect;

        private void myAnimatedCanvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var commandList = new CanvasCommandList(sender);
            using (var session = commandList.CreateDrawingSession())
            {
                for (int i = 0; i < 25; i++)
                {
                    session.DrawText("Hello, world!", RndPos(), RndColor());
                    session.DrawCircle(RndPos(), RndRad(), RndColor());
                    session.DrawLine(RndPos(), RndPos(), RndColor());
                }
            }

            blurEffect = new GaussianBlurEffect();
            blurEffect.Source = commandList;
            blurEffect.BlurAmount = 10.0f;
        }

        private void myAnimatedCanvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var radius = (float)(1 + Math.Sin(args.Timing.TotalTime.TotalSeconds)) * 10f;
            blurEffect.BlurAmount = radius;
            args.DrawingSession.DrawImage(blurEffect);
        }
    }
}
