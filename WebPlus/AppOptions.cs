namespace WebPlus
{
    public class AppOptions
    {
        public bool HotReload { get; set; }
        public bool SaveOnExit { get; set; }
        public bool DevToolsOnTop { get; set; }
        public bool OpenDevTools { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool StartInFullScreen { get; set; }
        public bool StartFrameless { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool Minimized { get; set; }

        public void Reset()
        {
            HotReload = true;
            SaveOnExit = true;
            DevToolsOnTop = true;
            OpenDevTools = true;
            Title = string.Empty;
            X = 100;
            Y = 100;
            Width = 960;
            Height = 540;
            StartInFullScreen = false;
            StartFrameless = false;
            MinimizeToTray = false;
            Minimized = false;
        }
        public override string ToString()
        {
            return $"HotReload:{HotReload}, SaveOnExit:{SaveOnExit}, OpenDevTools:{OpenDevTools},  DevToolsOnTop:{DevToolsOnTop}, Title:\"{Title}\", X:{X}, Y:{Y}, Width:{Width}, Height:{Height}, StartInFullScreen:{StartInFullScreen}, StartFrameless:{StartFrameless}, MinimizeToTray:{MinimizeToTray}, Minimized:{Minimized}";
        }
    }

}
