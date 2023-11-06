using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WebPlus
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class HostedObject
    {
        private Form1 HostForm;

        public bool InFullScreen = false;
        public bool EnteringOrExitingFullScreen;

        private bool hotReloadEnabled = false;
        private FileSystemWatcher watcher;
        private System.Timers.Timer timer;

        private string LastError = "";

        public HostedObject(Form1 hostForm)
        {
            HostForm = hostForm;
        }

        public void exit()
        {
            Application.Exit();
        }

        public string getPath()
        {
            return AppContext.BaseDirectory;
        }

        public string getLastError()
        {
            return LastError;
        }

        public void setWindowTitle(string title)
        {
            HostForm.Text = title;
        }

        public void setWindowIcon(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".ico":
                    HostForm.Icon = new Icon(path);
                    break;

                case ".png":
                    Image image = Image.FromFile(path);
                    Icon icon = Icon.FromHandle(new Bitmap(image).GetHicon());
                    HostForm.Icon = icon;
                    icon.Dispose();
                    image.Dispose();
                    break;

                default:
                    LastError = "Icon was not recognized.";
                    break;
            }
            HostForm.Icon = null;
            HostForm.ShowIcon = false;
        }

        public bool restoreHotReloadState()
        {
            enableHotReload(hotReloadEnabled);
            return hotReloadEnabled;
        }

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
            hotReloadEnabled = state;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            watcher.EnableRaisingEvents = true; // Re enable file system watch events after .5 seconds.
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string strFileExt = Path.GetExtension(e.FullPath);

            if (Regex.IsMatch(strFileExt, @"\.(html|js)$", RegexOptions.IgnoreCase))
            {
                HostForm.HotReload();

                watcher.EnableRaisingEvents = false; // Stop this event handler being fired for .5 seconds.
                timer.Start();
            }
        }

        public void setFullScreen(bool state)
        {
            if (state)
            {
                if (!InFullScreen)
                {
                    EnteringOrExitingFullScreen = true; // Stop form resize events sending extraneous resize messages.
                    HostForm.FormBorderStyle = FormBorderStyle.None;
                    HostForm.WindowState = FormWindowState.Maximized;
                    HostForm.ReplyToWebView("windowEnteredFullScreen");
                    EnteringOrExitingFullScreen = false;
                }
            }
            else
            {
                if (InFullScreen)
                {
                    EnteringOrExitingFullScreen = true;
                    HostForm.FormBorderStyle = FormBorderStyle.Sizable;
                    HostForm.WindowState = FormWindowState.Normal;
                    HostForm.ReplyToWebView("windowLeftFullScreen");
                    EnteringOrExitingFullScreen = false;
                }
            }
            InFullScreen = state;
        }

        public string fileInfo(string path)
        {
            try
            {
                return JsonSerializer.Serialize(new FileDetails(path));
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

                return JsonSerializer.Serialize(DirDetails.ToArray());
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
                        return JsonSerializer.Serialize(new FileDetails(openFileDialog.FileName));
                    }
                    else
                    {
                        var fileList = new List<FileDetails>();

                        foreach (String file in openFileDialog.FileNames)
                        {
                            fileList.Add(new FileDetails(file));
                        }
                        return JsonSerializer.Serialize(fileList);
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
                    return JsonSerializer.Serialize(new FileDetails(folderBrowserDialog.SelectedPath));
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

    }
}
