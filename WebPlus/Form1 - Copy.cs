using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WebView
{
    public partial class Form1 : Form
    {
        public JsonSerializerOptions JsonOptions = new JsonSerializerOptions { IncludeFields = true }; // Because by default it doesn't include fields ¯\_(ツ)_/¯

        private Boolean WasMinimized = false;
        private Boolean InFullScreen = false;

        public Form1()
        {
            InitializeComponent();
            InitializeAsync();
            webView.Source = new Uri($"file:///{Directory.GetCurrentDirectory()}/index.html");
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.OnResize(EventArgs.Empty);
        }

        /// <summary>
        /// Trigger initialization of the WebView control.
        /// </summary>
        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += MessageReceived;
            webView.CoreWebView2.OpenDevToolsWindow();

            webView.CoreWebView2.AddHostObjectToScript("hostedObject", new HostedObject(this));
        }

        /// <summary>
        /// Process messages received from thw WebView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void MessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String jsonString = args.TryGetWebMessageAsString();

            try
            {
                string[] request = JsonSerializer.Deserialize<string[]>(jsonString, JsonOptions);

                switch (request[0])
                {

                    // 
                    // Terminate the application.
                    // 

                    case "exit":
                    case "quit":
                    case "close":

                        Application.Exit();
                        break;
                        
                    // 
                    // Maximize the form.
                    // 

                    case "maximize":

                        this.WindowState = FormWindowState.Maximized;
                        break;

                    // 
                    // Minimize the form.
                    // 

                    case "minimize":

                        this.WindowState = FormWindowState.Minimized;
                        break;

                    // 
                    // 
                    // Enter Full Screen mode.
                    // 

                    case "enterFullScreen":

                        this.WindowState = FormWindowState.Normal;
                        FormBorderStyle = FormBorderStyle.None;
                        WindowState = FormWindowState.Maximized;
                        break;

                    // 
                    // 
                    // Leave Full Screen mode.
                    // 

                    case "leaveFullScreen":

                        this.Activate();
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        this.WindowState = FormWindowState.Normal;
                        break;

                    // 
                    // Set the forms title to the given text.
                    // 

                    case "setWindowTitle":

                        this.Text = request[1];
                        break;

                    // 
                    // Get information on the file with the given name.
                    // 

                    case "getFileInfo":
   
                        ReplyToWebView(new string[] {
                            "browseForAndLoadTextFile",
                            File.ReadAllText(request[1]), // File contents.
                            Path.GetFileName(request[1]), // File name.
                            Path.GetExtension(request[1]), // File extension.
                            Path.GetDirectoryName(request[1]), // Folder name.
                            Convert.ToString(new FileInfo(request[1]).Length) // Size of file.
                        });
                        break;
                        
                    // 
                    // Show a OpenFileDialog, and if not cancelled, load the selected file and return its contents, along with some extra information.
                    // 

                    case "browseForAndLoadTextFile":

                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = request[1],
                            FilterIndex = 0,
                            RestoreDirectory = true
                        };

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            ReplyToWebView(new string[] {
                                "browseForAndLoadTextFile",
                                File.ReadAllText(openFileDialog.FileName), // File contents.
                                Path.GetFileName(openFileDialog.FileName), // File name.
                                Path.GetExtension(openFileDialog.FileName), // File extension.
                                Path.GetDirectoryName(openFileDialog.FileName), // Folder name.
                                Convert.ToString(new FileInfo(openFileDialog.FileName).Length) // Size of file.
                            });
                        }
                        else
                        {
                            ReplyToWebView(new string[] { "browseForAndLoadTextFile" });
                        }
                        break;

                    // 
                    // Show a SaveFileDialog, and if not cancelled, save the given contents to the selected file.
                    // 

                    case "browseForAndSaveTextFile":

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = request[1],
                            FilterIndex = 0,
                            RestoreDirectory = true
                        };

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(saveFileDialog.FileName, request[2]);

                            ReplyToWebView(new string[] { "browseForAndSaveTextFile", "OK" });
                        }
                        else
                        {
                            ReplyToWebView(new string[] { "browseForAndLoadTextFile" });
                        }
                        break;

                    // 
                    // Uh oh.
                    // 

                    default:

                        MessageBox.Show($"MessageReceived(){Environment.NewLine}Unknown message received: {jsonString}");
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Send a reply to a request that was received from the WebView control.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        void ReplyToWebView(string[] reply)
        {
            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(reply, JsonOptions));
        }

        /// <summary>
        /// Resize WebView control to fill entire area of form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Resize(object sender, EventArgs e)
        {
            //webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);

            if (WindowState == FormWindowState.Minimized)
            {
                WasMinimized = true;
                ReplyToWebView(new string[] { "formResized", "minimized" });
            }
            else
            {
                if (WasMinimized) ReplyToWebView(new string[] { "formResized", "restored" });
                WasMinimized = false;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearBrowserCache();
        }

        /// <summary>
        /// Clear cached data on exit.
        /// </summary>
        private async void ClearBrowserCache()
        {
            await webView.CoreWebView2.Profile.ClearBrowsingDataAsync();
        }

    }
}
