using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Core;
using Windows.Foundation;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

namespace Sharemium
{
    public sealed partial class SharePage : Page
    {
        // Values for ShareDialogUI
        private string ShareContent;
        private string ShareTitle;
        private bool ShareTitleExists;
        private string ShareDescr;
        private string ShareHostApp;
        private bool ShareHostAppExists;

        // Values for ShareFinishedCountdown
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private int _progressValue = 0;
        private int _maxProgress = 15;

        public SharePage()
        {
            this.InitializeComponent();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(480, 120));
            ApplicationView.PreferredLaunchViewSize = new Size(480, 120);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(75, 154, 154, 154);
            ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(175, 154, 154, 154);
            ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareDialog_DataRequested;
            dataTransferManager.TargetApplicationChosen += ShareDialog_TargetApplicationChosen;

            _timer.Tick += Timer_Tick;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ProtocolHandlerParameters parameters)
            {
                HandleURIParameters(parameters.FullPath, parameters.QueryParams);
                await Task.Delay(150);
                DataTransferManager.ShowShareUI();
            }
        }

        public void HandleURIParameters(string fullPath, Dictionary<string, string> queryParams)
        {
            // Default sharing parameters
            ShareTitle = "Share";
            ShareTitleExists = false;
            ShareDescr = "Link sent using Sharemium";
            ShareHostAppExists = false;

            foreach (var param in queryParams)
            {
                switch (param.Key)
                {
                    case "title":
                        ShareTitle = param.Value;
                        if (ShareTitle != null)
                        {
                            ShareTitleExists = true;
                        }
                        break;
                    case "descr":
                        ShareDescr = param.Value;
                        break;
                    case "app":
                        ShareHostApp = param.Value;
                        if (ShareHostApp != null)
                        {
                            ShareHostAppExists = true;
                        }
                        break;
                }
            }

            // Adds the app name (if available) to the title if no title assigned
            if (ShareTitleExists == false && ShareHostAppExists == true)
            {
                ShareTitle = ShareTitle + " - from " + ShareHostApp;
            }

            // Add "https://" if missing
            if (!fullPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
                !fullPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                ShareContent = "https://" + fullPath;
            }
            else
            {
                ShareContent = fullPath;
            }

            // Output variables to debug textblock
            DebugBox.Text += ShareContent + "; ";
            DebugBox.Text += ShareTitle + "; ";
            DebugBox.Text += ShareDescr + "; ";
            DebugBox.Text += ShareHostApp + "; ";
        }

        public void ShareDialog_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = ShareTitle;
            request.Data.Properties.Description = ShareDescr;
            request.Data.SetWebLink(new Uri(ShareContent));
        }

        private void ShareDialog_TargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            SharingLoadText.Visibility = Visibility.Collapsed;
            SharingDonePanel.Visibility = Visibility.Visible;
            SharingProgress.IsIndeterminate = false;
            SharingProgress.Maximum = _maxProgress;
            SharingProgress.Value = _progressValue;
            _timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (_progressValue < _maxProgress)
            {
                SharingProgress.Value = ++_progressValue;
            }
            else
            {
                _timer.Stop();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CoreApplication.Exit());
            }
        }
    }
}
