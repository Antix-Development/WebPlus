using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WebPlus
{
    public partial class Form1 : Form
    {
        public JsonSerializerOptions JsonOptions = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }; // Options for when using objects with properties. By default properties are not serlialized.

        private HostedObject hostedObject; // The object containing the methods that are directly callable from JavaScript.

        public AppOptions appOptions;

        public bool WebViewInitiaitedClose = false;

        private string BaseAppFile = "app\\app";

        public Form1()
        {
            InitializeComponent();

            try
            {
                appOptions = JsonSerializer.Deserialize<AppOptions>(File.ReadAllText($"{BaseAppFile}.json"));
            }
            catch (Exception)
            {
                appOptions = new AppOptions();
                appOptions.Reset();
                saveOptions();
            }

            InitializeAsync();

            setIcon($"{BaseAppFile}.ico");

            // 
            // Make sure options are all set before loading "app.html", otherwise the WebPlus engine won't have access to some required properties, leading to unexpected behavior.
            // 

            WebView.Source = new Uri($"file:///{Directory.GetCurrentDirectory()}/app/app.html");

            this.Resize += new EventHandler(this.Form_Resize);
        }

        public bool setIcon(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".ico":
                    Icon = new Icon(path);
                    NotifyIcon1.Icon = new Icon(path);
                    return true;

                case ".png":
                    Image image = Image.FromFile(path);
                    Icon icon = Icon.FromHandle(new Bitmap(image).GetHicon());
                    Icon = icon;
                    NotifyIcon1.Icon = icon;
                    icon.Dispose();
                    image.Dispose();
                    return true;

                default:
                    hostedObject.LastError = "Icon was not recognized.";
                    return false;
            }
        }

        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);

            hostedObject = new HostedObject(this);
            WebView.CoreWebView2.AddHostObjectToScript("hostedObject", hostedObject);

            // 
            // Restore options
            // 

            hostedObject.enableHotReload(appOptions.HotReload);

            Location = new Point(appOptions.X, appOptions.Y);
            Size = new Size(appOptions.Width, appOptions.Height);
            MaximumSize = new Size(appOptions.MinimumWidth, appOptions.MinimumHeight);
            MinimumSize = new Size(appOptions.MaximumWidth, appOptions.MaximumHeight);

            SetWindowTitle(appOptions.Title);

            hostedObject.minimizeToTray(appOptions.MinimizeToTray);

            hostedObject.startInFullScreen(appOptions.StartInFullScreen);

            hostedObject.CanResize = appOptions.CanResize;

            if (!appOptions.CanResize)
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                MaximizeBox = false;
            }

            if (appOptions.StartInFullScreen)
            {
                hostedObject.setFullScreen(true);
            }

            else if (appOptions.StartFrameless)
            {
                FormBorderStyle = FormBorderStyle.None;
            }



            if (appOptions.OpenDevTools)
            {
                WebView.CoreWebView2.OpenDevToolsWindow();
                WantToOpenDevTools = true;
            }

            //WebView.CoreWebView2.WebMessageReceived += MessageReceived;
        }

        private bool WantToOpenDevTools = false;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        const int WM_CLOSE = 0x0010;

        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOACTIVATE = 0x0010;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private IntPtr DevToolsHandle = IntPtr.Zero;

        /// <summary>
        /// Get handle to dcevtools window if it is open.
        /// </summary>
        /// <returns></returns>
        public bool GetDevToolsHandle()
        {
            if (DevToolsHandle != IntPtr.Zero) return true; // Return if it was previously found and not reset.

            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.ProcessName == "msedgewebview2" & pList.MainWindowTitle.IndexOf("DevTools") == 0)
                {
                    DevToolsHandle = pList.MainWindowHandle;
                    return true;
                }
            }
            DevToolsHandle = IntPtr.Zero;
            return false;
        }

        /// <summary>
        /// If the devtools window is open, bring it to the top.
        /// </summary>
        private void DevToolsToFront()
        {
            if (GetDevToolsHandle()) SetWindowPos(DevToolsHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        /// <summary>
        /// Bring the devtools window to the top when the form is activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Activated(object sender, EventArgs e)
        {
            DevToolsToFront();
        }

        /// <summary>
        /// Raise a new "windowclose" event on the web app side.
        /// </summary>
        /// <param name="type"></param>
        //public void DispatchWindowCloseEvent(string type)
        //{
        //    var script = $"var event = new CustomEvent('windowclose'); window.dispatchEvent(event);";
        //    WebView.CoreWebView2.ExecuteScriptAsync(script);
        //}
        
        /// <summary>
        /// Raise a new "windowresize" event on the web app side.
        /// </summary>
        /// <param name="type"></param>
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
                        NotifyIcon1.Visible = true;
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

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            NotifyIcon1.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearBrowserCache();
        }

        public void SetWindowTitle(string title)
        {
            Text = title;
            NotifyIcon1.Text = title;
        }

        private async void ClearBrowserCache()
        {
            await WebView.CoreWebView2.Profile.ClearBrowsingDataAsync();
        }

        public void HotReload()
        {
            this.Invoke(new Action(() => {
                WebView.CoreWebView2.Reload();
            }));
        }

        private void webView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

                case Keys.F11:
                    if (appOptions.CanResize) e.Handled = true;
                    break;

                case Keys.F5:
                    e.Handled = true;
                    break;

                case Keys.F12:
                    if (appOptions.DevToolsOnTop) e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        private void WebView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    WebView.CoreWebView2.Reload();
                    e.Handled = true;
                    break;

                case Keys.F11:
                    if (appOptions.CanResize)
                    {
                        if (hostedObject.InFullScreen)
                        {
                            hostedObject.setFullScreen(false);
                        }
                        else
                        {
                            hostedObject.setFullScreen(true);
                        }
                        e.Handled = true;
                    }
                    break;

                case Keys.F12:
                    if (appOptions.DevToolsOnTop)
                    {
                        WantToOpenDevTools = !WantToOpenDevTools;
                        //ShowDevTools(DevToolsOpen);

                        if (WantToOpenDevTools)
                        {
                            // We want to open "DevTools".
                            if (DevToolsHandle != IntPtr.Zero)
                            {
                                DevToolsToFront(); // "DevTools" is already open, raise it to the top.
                            }
                            else
                            {
                                WebView.CoreWebView2.OpenDevToolsWindow(); // Open "DevTools".
                            }
                        }
                        else
                        {
                            // We want to close "DevTools".
                            if (GetDevToolsHandle())
                            {
                                // If "DevTools" is open, ask it to close.
                                SendMessage(DevToolsHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                DevToolsHandle = IntPtr.Zero;
                                WantToOpenDevTools = false;
                            }
                        }
                        e.Handled = true;
                    }
                    break;

                default:
                    break;
            }
        }
        public async Task Execute(Action action, int timeoutInMilliseconds)
        {
            await Task.Delay(timeoutInMilliseconds);
            action();
        }

        private void saveOptions()
        {
            File.WriteAllText($"{BaseAppFile}.json", JsonSerializer.Serialize(appOptions, JsonOptions));
        }

        /// <summary>
        /// Persist state before the form closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!WebViewInitiaitedClose & appOptions.DelegateCloseEvent)
            {
                var script = $"var event = new CustomEvent('windowclose'); window.dispatchEvent(event);";
                WebView.CoreWebView2.ExecuteScriptAsync(script);
                e.Cancel = true;
                return;
            }

            if (appOptions.SaveOnExit)
            {
                if (WindowState != FormWindowState.Minimized) // Only persist the forms `Location` and `Size` if the form is NOT mimimized.
                {
                    appOptions.X = Location.X;
                    appOptions.Y = Location.Y;
                    appOptions.Width = Size.Width;
                    appOptions.Height = Size.Height;
                }

                appOptions.Title = Text;
                appOptions.StartInFullScreen = hostedObject.StartInFullScreen;
                appOptions.StartFrameless = hostedObject.StartFrameless;
                appOptions.Minimized = hostedObject.Minimized;
                appOptions.MinimizeToTray  = hostedObject.MinimizeToTray;

                saveOptions();
            }
        }

    }
}
