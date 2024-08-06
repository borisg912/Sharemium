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

namespace Sharemium
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ForegroundColor = Color.FromArgb(255, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Color.FromArgb(255, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(75, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(175, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.InactiveForegroundColor = Color.FromArgb(255, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 0, 0, 0);
        }
    }
}
