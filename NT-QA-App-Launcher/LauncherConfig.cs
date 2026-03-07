namespace NTQAAppLauncher
{
    /// <summary>
    /// Centralized configuration constants for the NT Q&A App Launcher
    /// </summary>
    public static class LauncherConfig
    {
        public const string SERVER_NAME = "NT Q&A App";
        public const string APP_PATH = @"D:\NT-QA-App-Project\dist\NT-QA-App-Setup\app";
        public const string NPM_START_COMMAND = "npm start";
        public const int DEFAULT_PORT = 3000;
        public const string BASE_URL = "http://localhost:3000";
        public const int SERVER_CHECK_INTERVAL_MS = 2000;
        public const int SERVER_START_TIMEOUT_MS = 10000;

        public static class UI
        {
            public const int WINDOW_WIDTH = 500;
            public const int WINDOW_HEIGHT = 350;
            public const string WINDOW_TITLE = "NT Q&A Application Launcher";
            public const int PADDING = 10;
            public const int BUTTON_WIDTH = 120;
            public const int BUTTON_HEIGHT = 35;
            public const int LABEL_HEIGHT = 25;
        }
    }
}
