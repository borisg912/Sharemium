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
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sharemium
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(75, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(175, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.PreferredLaunchViewSize = new Size(600, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async void DemoButtonFinal_Click(object sender, RoutedEventArgs e)
        {
            var DemoPageURL = new Uri(@"https://borisg912.github.io/Sharemium/DemoLaunch/");
            var DemoPageLoad = await Windows.System.Launcher.LaunchUriAsync(DemoPageURL);
            if (DemoPageLoad)
            {
                DemoButtonClickedOutput.Visibility = Visibility.Visible;
                DemoButtonClickedOutput.Text = "Demo page open";
            }
            else
            {
                DemoButtonClickedOutput.Visibility = Visibility.Visible;
                DemoButtonClickedOutput.Text = "Couldn't open demo page, @" + DemoPageURL;
            }
        }
        private void DebugTemp(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SharePage));
        }
    }
}
