using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Sharemium
{
    sealed partial class App : Application
    {
        private readonly List<string> expectedParams = new List<string> { "title", "descr", "app" };

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                if (protocolArgs != null)
                {
                    HandleProtocolActivation(protocolArgs.Uri);
                } else { LaunchMainPage(); }
            }
            base.OnActivated(args);
        }

        private void HandleProtocolActivation(Uri uri)
        {
            if (uri == null || (string.IsNullOrEmpty(uri.AbsolutePath.Trim('/')) && string.IsNullOrEmpty(uri.Query)))
            {
                LaunchMainPage();
                return;
            }
            var fullPath = uri.AbsolutePath.Trim('/');
            var queryParams = new Dictionary<string, string>();
            var queryString = uri.Query;
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.Substring(1); // Remove the leading '?'
                var pairs = queryString.Split('&');
                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        var key = Uri.UnescapeDataString(keyValue[0]);
                        var value = Uri.UnescapeDataString(keyValue[1]);
                        if (expectedParams.Contains(key))
                        {
                            queryParams[key] = value;
                        }
                    }
                }
            }
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(SharePage), new ProtocolHandlerParameters(fullPath, queryParams));
            Window.Current.Activate();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            LaunchMainPage();
        }

        private void LaunchMainPage()
        {
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
                
            }
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
    public class ProtocolHandlerParameters
    {
        public string FullPath { get; set; }
        public Dictionary<string, string> QueryParams { get; set; }

        public ProtocolHandlerParameters(string fullPath, Dictionary<string, string> queryParams)
        {
            FullPath = fullPath;
            QueryParams = queryParams;
        }
    }
}
