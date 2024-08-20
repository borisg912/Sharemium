using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
using System.Collections;
using Windows.System;

namespace Sharemium
{
    sealed partial class App : Application
    {
        private readonly List<string> ParamWhitelist = new List<string> { "title", "descr", "app" };

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                Uri uri = protocolArgs?.Uri;
                if (uri != null && !string.IsNullOrEmpty(uri.AbsolutePath))
                {
                    HandleProtocolActivation(uri);
                }
                else
                {
                    EmptyProtocolLaunch();
                }
            }
            base.OnActivated(args);
        }

        private void HandleProtocolActivation(Uri uri)
        {
            if (uri == null || (string.IsNullOrEmpty(uri.AbsolutePath.Trim('/')) && string.IsNullOrEmpty(uri.Query)))
            {
                EmptyProtocolLaunch();
                return;
            }
            string[] urlParts = uri.ToString().Split(new[] { '#' }, 2);
            string baseUrlWithParams = urlParts[0];
            Uri baseUri = new Uri(baseUrlWithParams);
            string baseURL = $"{baseUri.Host}{baseUri.AbsolutePath}";

            var QueryList = ExtractParameters(baseUri.Query);
            Dictionary<string, string> appParams = new Dictionary<string, string>();
            if (urlParts.Length > 1) // Only after '#'
            {
                appParams = ExtractParameters(urlParts[1], ParamWhitelist);
            }

            var fullPath = baseURL;
            if (QueryList.Count > 0)
            {
                fullPath += "?" + string.Join("&", QueryList.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            }
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(SharePage), new ProtocolHandlerParameters(fullPath, appParams));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(420, 420));
            ApplicationView.PreferredLaunchViewSize = new Size(420, 620);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Window.Current.Activate();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            EmptyProtocolLaunch();
        }
        private async void EmptyProtocolLaunch()
        {
            await Launcher.LaunchUriAsync(new Uri(@"https://borisg912.github.io/Sharemium"));
            Exit();
        }
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        private Dictionary<string, string> ExtractParameters(string query, List<string> whitelist = null)
        {
            var parameters = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(query)) return parameters;
            if (query.StartsWith("?"))
            {
                query = query.Substring(1); // Question mark '?'
            }
            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = Uri.UnescapeDataString(keyValue[0]);
                    var value = Uri.UnescapeDataString(keyValue[1]);
                    if (whitelist == null || whitelist.Contains(key))
                    {
                        parameters[key] = value;
                    }
                }
            }
            return parameters;
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
