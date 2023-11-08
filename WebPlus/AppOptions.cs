namespace WebPlus
{
    public class AppOptions
    {
        public bool HotReload { get; set; }
        public bool SaveOnExit { get; set; }
        public bool UseDevTools { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool FullScreen { get; set; }
        public bool Frameless { get; set; }
        public bool Minimized { get; set; }
        public bool MinimizeToTray { get; set; }

        public void Reset()
        {
            HotReload = true;
            SaveOnExit = false;
            UseDevTools = false;
            Title = string.Empty;
            X = 100;
            Y = 100;
            Width = 960;
            Height = 540;
            FullScreen = false;
            Frameless = false;
            MinimizeToTray = false;
        }
        public override string ToString()
        {
            return $"HotReload:{HotReload}, SaveOnExit:{SaveOnExit}, UseDevTools:{UseDevTools}, Title:\"{Title}\", X:{X}, Y:{Y}, Width:{Width}, Height:{Height}, FullScreen:{FullScreen}, Frameless:{Frameless}, MinimizeToTray:{MinimizeToTray}";
        }
    }

}
