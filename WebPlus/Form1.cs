using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WebPlus
{
    public partial class Form1 : Form
    {
        public JsonSerializerOptions JsonOptions = new JsonSerializerOptions { IncludeFields = true }; // Options for when using objects with properties. By default properties are not serlialized.

        private HostedObject hostedObject; // The object containing the methods that are directly callable from JavaScript.

        public Form1()
        {
            InitializeComponent();

            try
            {
                this.Icon = new Icon("app\\icon.ico");
            }
            catch (Exception)
            {
            }

            InitializeAsync();
            WebView.Source = new Uri($"file:///{Directory.GetCurrentDirectory()}/app/app.html");
            this.Resize += new EventHandler(this.Form_Resize);
        }

        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
            WebView.CoreWebView2.WebMessageReceived += MessageReceived;

            WebView.CoreWebView2.OpenDevToolsWindow();

            hostedObject = new HostedObject(this);
            WebView.CoreWebView2.AddHostObjectToScript("hostedObject", hostedObject);
        }

        void MessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String jsonString = args.TryGetWebMessageAsString();

            try
            {
                string[] request = JsonSerializer.Deserialize<string[]>(jsonString, JsonOptions);

                switch (request)
                {
                    default:
                        Console.WriteLine($"unmanaged message received: {request}");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"invalid message received: {jsonString}");
            }
        }

        public void ReplyToWebView(string reply)
        {
            WebView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(reply, JsonOptions));
        }

        public void DispatchWindowResizeEvent(string type)
        {
            var script = $"window.dispatchEvent(new CustomEvent('windowresize', {{detail: {type} }}));";
            WebView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            WebView.CoreWebView2.OpenDevToolsWindow();

            if (hostedObject.EnteringOrExitingFullScreen) return;

            switch (WindowState)
            {
                case FormWindowState.Normal:
                    ReplyToWebView("windowRestored");
                    DispatchWindowResizeEvent("windowRestored");
                    break;

                case FormWindowState.Minimized:
                    DispatchWindowResizeEvent("windowMinimized");
                    ReplyToWebView("windowMinimized");
                    //WasMinimized = true;
                    break;

                case FormWindowState.Maximized:
                    if (!hostedObject.InFullScreen)
                    {
                        DispatchWindowResizeEvent("windowMaximized");
                        ReplyToWebView($"windowMaximized");
                    }
                    break;

                default:
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearBrowserCache();
        }

        private async void ClearBrowserCache()
        {
            await WebView.CoreWebView2.Profile.ClearBrowsingDataAsync();
        }

        private void webView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                case Keys.F11:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        public void HotReload()
        {
            this.Invoke(new Action(() => {
                WebView.CoreWebView2.Reload();
            }));
        }

        private void webView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    WebView.CoreWebView2.Reload();
                    e.Handled = true;
                    break;

                case Keys.F11:
                    if (hostedObject.InFullScreen)
                    {
                        hostedObject.setFullScreen(false);
                    }
                    else
                    {
                        hostedObject.setFullScreen(true);
                    }
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

    }
}
