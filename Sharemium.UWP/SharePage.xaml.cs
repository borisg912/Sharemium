using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.Foundation;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

namespace Sharemium
{
    public sealed partial class SharePage : Page
    {
        private string ShareTypeContent;
        private string ShareTitle;
        private string ShareDescr;
        private string ShareLink;
        public SharePage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(75, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(175, 254, 254, 254);
            ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ApplicationView.PreferredLaunchViewSize = new Size(500, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            dataTransferManager.TargetApplicationChosen += DataTransferManager_TargetApplicationChosen;
        }
        public void HandleURIParameters(string fullPath, Dictionary<string, string> queryParams)
        {
            ShareTitle = "Website";
            ShareDescr = "Link sent using Sharemium";
            ShareLink = fullPath;
            foreach (var param in queryParams)
            {
                switch (param.Key)
                {
                    case "typeof":
                        ShareTypeContent = param.Value;
                        break;
                    case "title":
                        ShareTitle = param.Value;
                        break;
                    case "descr":
                        ShareDescr = param.Value;
                        break;
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ProtocolHandlerParameters parameters)
            {
                HandleURIParameters(parameters.FullPath, parameters.QueryParams);
                DataTransferManager.ShowShareUI();
            }
        }
        public void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = ShareTitle;
            request.Data.Properties.Description = ShareDescr;
            request.Data.SetWebLink(new Uri(ShareLink));
        }
        private void DataTransferManager_TargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            return;
        }
    }
}
