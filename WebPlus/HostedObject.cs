using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WebPlus
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class HostedObject
    {
        private bool HotReloadEnabled = false;

        public bool MinimizeToTray = false;
        public bool Minimized = false;
        public bool UseDevTools = false;

        public bool CanResize = true;

        public bool Frameless = false;
        public bool StartFrameless = false;

        public bool InFullScreen = false;
        public bool StartInFullScreen = false;

        public bool IgnoreResizingEvents;
        private FileSystemWatcher watcher;
        private System.Timers.Timer timer;
        public string LastError = "";

        private JsonSerializerOptions JsonOptions = new JsonSerializerOptions { IncludeFields = true };
        private readonly Form1 HostForm;

        public HostedObject(Form1 hostForm)
        {
            HostForm = hostForm;
        }

        public void exit()
        {
            HostForm.WebViewInitiaitedClose = true;

            Application.Exit();
        }

        public string getPath()
        {
            return AppContext.BaseDirectory;
        }

        public string getLastError()
        {
            var lastError = LastError;
            LastError = "";
            return lastError;
        }


        public void setWindowTitle(string title)
        {
            HostForm.SetWindowTitle(title);
        }

        public bool setWindowIcon(string path)
        {
            return HostForm.setIcon(path);
        }


        public bool getMinimizeToTrayState()
        {
            return MinimizeToTray;
        }

        public void minimizeToTray(bool state)
        {
            MinimizeToTray = state;
        }

        #region WinForm Size and Location.

        /// <summary>
        /// Set the winforms location to the given screen coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setWindowLocation(int x, int y)
        {
            HostForm.Location = new Point(x, y);
        }

        /// <summary>
        /// Set the winforms size to the given dimensions.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void setWindowSize(int width, int height)
        {
            HostForm.Size = new Size(width, height);
        }

        /// <summary>
        /// Get the bounds of the winform.
        /// </summary>
        /// <returns></returns>
        public string getWindowBounds()
        {
            return $"{{\"x\":{HostForm.Location.X},\"y\":{HostForm.Location.Y},\"width\":{HostForm.Size.Width},\"height\":{HostForm.Size.Height},\"innerWidth\":{HostForm.ClientSize.Width},\"innerHeight\":{HostForm.ClientSize.Height}}}";
        }

        #endregion

        #region Full-screen mode management.

        /// <summary>
        /// Set the app to launch in full-screen or windowed mode according to the given state.
        /// </summary>
        /// <param name="state"></param>
        public void startInFullScreen(bool state)
        {
            StartInFullScreen = state;
        }

        /// <summary>
        /// Get the current full-screen state.
        /// </summary>
        /// <returns></returns>
        public bool getFullScreenState()
        {
            return InFullScreen;
        }

        /// <summary>
        /// Enter or leave full-screen mode according to the given state.
        /// </summary>
        /// <param name="state"></param>
        public void setFullScreen(bool state)
        {
            if (state)
            {
                if (!InFullScreen)
                {
                    IgnoreResizingEvents = true; // Stop form resize events sending extraneous resize messages.
                    HostForm.FormBorderStyle = FormBorderStyle.None;
                    HostForm.WindowState = FormWindowState.Maximized;
                    HostForm.DispatchWindowResizeEvent("windowEnteredFullScreen");
                    IgnoreResizingEvents = false;
                }
            }
            else
            {
                if (InFullScreen)
                {
                    IgnoreResizingEvents = true;
                    if (!Frameless) HostForm.FormBorderStyle = (CanResize) ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
                    HostForm.WindowState = FormWindowState.Normal;
                    HostForm.DispatchWindowResizeEvent("windowLeftFullScreen");
                    IgnoreResizingEvents = false;
                }
            }
            InFullScreen = state;
        }

        #endregion

        #region WinForm frameless mode management.

        /// <summary>
        /// Set the app to START in frameless or framed mode according to the given state.
        /// </summary>
        /// <param name="state"></param>
        public void startFrameless(bool state)
        {
            StartFrameless = state;
        }

        /// <summary>
        /// Set the app to BE frameless or framed mode according to the given state.
        /// </summary>
        /// <param name="state"></param>
        public void setFrameless(bool state)
        {
            IgnoreResizingEvents = true;
            if (state)
            {
                HostForm.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                if (!InFullScreen) HostForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            IgnoreResizingEvents = false;

            Frameless = state;
        }

        /// <summary>
        /// Get the current frameless state of the app.
        /// </summary>
        /// <returns></returns>
        public bool getFrameLessState()
        {
            return Frameless;
        }

        #endregion

        #region Hot Reloading

        /// <summary>
        /// Restore hot reload state on app reload.
        /// </summary>
        /// <returns></returns>
        public bool restoreHotReloadState()
        {
            enableHotReload(HotReloadEnabled);
            return HotReloadEnabled;
        }

        /// <summary>
        /// Enable or disable hot reloading according to the given state.
        /// </summary>
        /// <param name="state"></param>
        public void enableHotReload(bool state)
        {
            if (state)
            {
                if (watcher == null)
                {
                    timer = new System.Timers.Timer();
                    timer.Interval = 500;
                    timer.AutoReset = false;
                    timer.Elapsed += Timer_Elapsed;

                    watcher = new FileSystemWatcher();
                    watcher.Path = Path.Combine(getPath(), "app");
                    watcher.Filter = "";
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Changed += Watcher_Changed;
                }
                watcher.EnableRaisingEvents = true;
            }
            else
            {
                if (watcher != null)
                {
                    watcher.EnableRaisingEvents = false;
                    timer.Stop(); // Just in case it was already counting down.
                }
            }
            HotReloadEnabled = state;
        }

        /// <summary>
        /// Re-enable filesystemwatcher events on timer expired.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            watcher.EnableRaisingEvents = true; // Re enable file system watch events after .5 seconds.
        }

        /// <summary>
        /// The file system watcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string strFileExt = Path.GetExtension(e.FullPath);

            if (Regex.IsMatch(strFileExt, @"\.(html|js|css)$", RegexOptions.IgnoreCase))
            {
                HostForm.HotReload(); // Restart the web app.

                watcher.EnableRaisingEvents = false; // Stop this event handler being fired for half a second to stop multiple events being raised in quick succession.
                timer.Start();
            }
        }

        #endregion

        public void launchProcess(string uri)
        {
            try
            {
                Process.Start(uri);

            }
            catch (Exception)
            {
            }
        }

        public string fileInfo(string path)
        {
            try
            {
                return JsonSerializer.Serialize(new FileDetails(path), JsonOptions);
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public string dirInfo(string path)
        {
            try
            {
                var DirDetails = new List<FileDetails>();
                var files = from file in Directory.EnumerateFiles(path) select file;
                foreach (var file in files)
                {
                    DirDetails.Add(new FileDetails(Path.Combine(path, file)));
                }

                string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    DirDetails.Add(new FileDetails(Path.Combine(path, dir)));
                }

                return JsonSerializer.Serialize(DirDetails.ToArray(), JsonOptions);
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public string loadTextFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return null;
            }
        }
        
        public bool saveTextFile(string text, string path)
        {
            try
            {
               File.WriteAllText(path, text);
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message + " for fucks sake!";
                return false;
            }
        }

        public string openFileDialog(string options)
        {
            var dialogOptions = JsonSerializer.Deserialize<FileDialogOptions>(options);

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = dialogOptions.filter,
                    Title = dialogOptions.title,
                    Multiselect = dialogOptions.multiSelect,
                    FilterIndex = 0,
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!dialogOptions.multiSelect)
                    {
                        return JsonSerializer.Serialize(new FileDetails(openFileDialog.FileName), JsonOptions);
                    }
                    else
                    {
                        var fileList = new List<FileDetails>();

                        foreach (String file in openFileDialog.FileNames)
                        {
                            fileList.Add(new FileDetails(file));
                        }
                        return JsonSerializer.Serialize(fileList, JsonOptions);
                    }
                }
            }
            catch (Exception e)
            {
                LastError = e.Message;
            }
            return null;
        }

        public string openFolderDialog()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true
                };

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    return JsonSerializer.Serialize(new FileDetails(folderBrowserDialog.SelectedPath), JsonOptions);
                }
            }
            catch (Exception e)
            {
                LastError = e.Message;
            }
            return null;
        }

        public string brosweForAndLoadTextFile(string options)
        {
            var dialogOptions = JsonSerializer.Deserialize<FileDialogOptions>(options);

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = dialogOptions.filter,
                    FilterIndex = 0,
                    Title = dialogOptions.title,
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return File.ReadAllText(openFileDialog.FileName);
                }
            }
            catch (Exception e)
            {
                LastError = e.Message;
            }
            return null;
        }

        public bool browseForAndSaveTextFile(string text, string options)
        {
            var dialogOptions = JsonSerializer.Deserialize<FileDialogOptions>(options);

            try
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = dialogOptions.filter,
                    Title = dialogOptions.title,
                    FilterIndex = 0,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, text);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public string saveFileDialog(string options)
        {
            var dialogOptions = JsonSerializer.Deserialize<FileDialogOptions>(options);

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = dialogOptions.filter,
                    Title = dialogOptions.title,
                    FilterIndex = 0,
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return JsonSerializer.Serialize(new FileDetails(saveFileDialog.FileName), JsonOptions);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public bool deleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public bool renameFile(string path, string newName)
        {
            try
            {
                File.Move(path, Path.Combine(Path.GetDirectoryName(path), newName));
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }
        
        public bool createDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public bool renameDirectory(string path, string newName)
        {
            try
            {
                var di = new DirectoryInfo(path);
                Directory.Move(path, Path.Combine(di.Parent.FullName, newName));
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public bool deleteDirectory(string path)
        {
            try
            {
                RecursiveDelete(new DirectoryInfo(path));
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            var files = baseDir.GetFiles();
            foreach (var file in files)
            {
                file.IsReadOnly = false;
                file.Delete();
            }
            baseDir.Delete();
        }

        public bool savePNG(string dataURL, string path)
        {
            //Console.WriteLine(dataURL);
            //Console.WriteLine(path);

            string base64String = dataURL.Replace("data:image/png;base64,", "");

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                File.WriteAllBytes(path, imageBytes);
                return true;
            }
            catch (Exception e)
            {
                LastError = e.Message;
                return false;
            }
        }
    }
}
