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

            setIcon("app\\icon.ico");

            notifyIcon1.Text = this.Text;

            InitializeAsync();
            WebView.Source = new Uri($"file:///{Directory.GetCurrentDirectory()}/app/app.html");
            this.Resize += new EventHandler(this.Form_Resize);
        }

        public string setIcon(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".ico":
                    this.Icon = new Icon(path);
                    notifyIcon1.Icon = new Icon(path);
                    return null;

                case ".png":
                    Image image = Image.FromFile(path);
                    Icon icon = Icon.FromHandle(new Bitmap(image).GetHicon());
                    this.Icon = icon;
                    notifyIcon1.Icon = icon;
                    icon.Dispose();
                    image.Dispose();
                    return null;

                default:
                    return "Icon was not recognized.";
            }
        }

        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);

            hostedObject = new HostedObject(this);
            WebView.CoreWebView2.AddHostObjectToScript("hostedObject", hostedObject);

            //WebView.CoreWebView2.WebMessageReceived += MessageReceived;
        }

        //void MessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        //{
        //    String jsonString = args.TryGetWebMessageAsString();

        //    try
        //    {
        //        string[] request = JsonSerializer.Deserialize<string[]>(jsonString, JsonOptions);

        //        switch (request)
        //        {
        //            default:
        //                Console.WriteLine($"unmanaged message received: {request}");
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine($"invalid message received: {jsonString}");
        //    }
        //}

        //public void ReplyToWebView(string reply)
        //{
        //    WebView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(reply, JsonOptions));
        //}

        public void DispatchWindowResizeEvent(string type)
        {
            var script = $"var event = new CustomEvent('windowresize', {{detail: '{type}' }}); window.dispatchEvent(event);";
            WebView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (hostedObject.IgnoreResizingEvents) return;

            switch (WindowState)
            {
                case FormWindowState.Normal:
                    DispatchWindowResizeEvent("windowRestored");
                    break;

                case FormWindowState.Minimized:
                    if (hostedObject.MinimizeToTray)
                    {
                        Hide();
                        notifyIcon1.Visible = true;
                    }
                    DispatchWindowResizeEvent("windowMinimized");
                    break;

                case FormWindowState.Maximized:
                    if (!hostedObject.InFullScreen)
                    {
                        DispatchWindowResizeEvent("windowMaximized");
                    }
                    break;

                default:
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearBrowserCache();
        }

        public void SetWindowTitle(string title)
        {
            this.Text = title;
            notifyIcon1.Text = title;
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
