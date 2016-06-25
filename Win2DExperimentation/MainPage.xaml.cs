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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Win2DExperimentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            sender.DoubleTapped += Sender_DoubleTapped;
            args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            args.DrawingSession.DrawText("Hello, world!", 100, 100, Colors.Yellow);
        }

        private async void Sender_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var dialog = new MessageDialog("You double tapped!");
            dialog.Title = "MessageDialog test";
            dialog.Commands.Add(new UICommand { Label = "Sure did!", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Maybe...", Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var res = await dialog.ShowAsync();

            switch ((int)res.Id)
            {
                case 0:
                    await new MessageDialog("You seem confident.").ShowAsync();
                    break;
                case 1:
                    break;
            }
        }
    }
}
