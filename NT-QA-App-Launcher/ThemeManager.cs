using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Manages Windows 11 theme colors and styling
    /// </summary>
    public static class ThemeManager
    {
        [DllImport("uxtheme.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars,
            StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int dwMaxSizeChars);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend);

        // Windows 11 Modern Colors
        public static class Colors
        {
            // Light Mode
            public static readonly Color LightBackground = Color.FromArgb(255, 255, 255);
            public static readonly Color LightForeground = Color.FromArgb(32, 32, 32);
            public static readonly Color LightSurface = Color.FromArgb(243, 243, 243);
            public static readonly Color LightBorder = Color.FromArgb(229, 229, 229);

            // Dark Mode
            public static readonly Color DarkBackground = Color.FromArgb(32, 32, 32);
            public static readonly Color DarkForeground = Color.FromArgb(255, 255, 255);
            public static readonly Color DarkSurface = Color.FromArgb(45, 45, 45);
            public static readonly Color DarkBorder = Color.FromArgb(64, 64, 64);

            // Status Colors
            public static readonly Color Success = Color.FromArgb(16, 185, 129);    // Green
            public static readonly Color Warning = Color.FromArgb(245, 158, 11);    // Orange
            public static readonly Color Error = Color.FromArgb(239, 68, 68);       // Red
            public static readonly Color Info = Color.FromArgb(59, 130, 246);       // Blue
            public static readonly Color Neutral = Color.FromArgb(107, 114, 128);   // Gray

            // Accent (Windows system color)
            public static Color AccentColor { get; set; } = Color.FromArgb(0, 120, 215);
        }

        public static class Fonts
        {
            public static readonly Font TitleFont = new Font("Segoe UI", 18, FontStyle.Bold);
            public static readonly Font HeadingFont = new Font("Segoe UI", 12, FontStyle.Bold);
            public static readonly Font LabelFont = new Font("Segoe UI", 11);
            public static readonly Font NormalFont = new Font("Segoe UI", 10);
            public static readonly Font SmallFont = new Font("Segoe UI", 9);
            public static readonly Font MonospaceFont = new Font("Consolas", 9);
        }

        // Theme mode
        public enum ThemeMode
        {
            Light,
            Dark,
            Auto
        }

        public static ThemeMode CurrentTheme { get; set; } = ThemeMode.Auto;
        public static bool IsDarkMode { get; private set; }

        static ThemeManager()
        {
            DetectSystemTheme();
            UpdateAccentColor();
        }

        /// <summary>
        /// Detect if Windows is in dark mode
        /// </summary>
        public static void DetectSystemTheme()
        {
            if (CurrentTheme == ThemeMode.Auto)
            {
                try
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                    {
                        var value = key?.GetValue("AppsUseLightTheme");
                        IsDarkMode = value != null && (int)value == 0;
                    }
                }
                catch
                {
                    IsDarkMode = false; // Default to light
                }
            }
            else
            {
                IsDarkMode = CurrentTheme == ThemeMode.Dark;
            }
        }

        /// <summary>
        /// Get Windows system accent color
        /// </summary>
        private static void UpdateAccentColor()
        {
            try
            {
                if (DwmGetColorizationColor(out uint color, out _) == 0)
                {
                    Colors.AccentColor = Color.FromArgb((int)(color | 0xFF000000));
                }
            }
            catch
            {
                // Use default if failed
                Colors.AccentColor = Color.FromArgb(0, 120, 215);
            }
        }

        /// <summary>
        /// Get background color based on current theme
        /// </summary>
        public static Color GetBackgroundColor()
        {
            return IsDarkMode ? Colors.DarkBackground : Colors.LightBackground;
        }

        /// <summary>
        /// Get foreground/text color based on current theme
        /// </summary>
        public static Color GetForegroundColor()
        {
            return IsDarkMode ? Colors.DarkForeground : Colors.LightForeground;
        }

        /// <summary>
        /// Get surface color for panels/cards
        /// </summary>
        public static Color GetSurfaceColor()
        {
            return IsDarkMode ? Colors.DarkSurface : Colors.LightSurface;
        }

        /// <summary>
        /// Get border color
        /// </summary>
        public static Color GetBorderColor()
        {
            return IsDarkMode ? Colors.DarkBorder : Colors.LightBorder;
        }

        /// <summary>
        /// Apply theme to a control
        /// </summary>
        public static void ApplyTheme(Control control)
        {
            control.BackColor = GetBackgroundColor();
            control.ForeColor = GetForegroundColor();

            foreach (Control child in GetAllControls(control))
            {
                if (child is Panel panel)
                {
                    panel.BackColor = GetSurfaceColor();
                    panel.ForeColor = GetForegroundColor();
                }
                else if (child is Label label)
                {
                    label.BackColor = Color.Transparent;
                    label.ForeColor = GetForegroundColor();
                }
                else if (child is Button button)
                {
                    button.ForeColor = GetForegroundColor();
                    button.BackColor = Colors.AccentColor;
                }
                else if (child is TextBox textBox)
                {
                    textBox.BackColor = GetBackgroundColor();
                    textBox.ForeColor = GetForegroundColor();
                }
                else if (child is RichTextBox richText)
                {
                    richText.BackColor = GetBackgroundColor();
                    richText.ForeColor = GetForegroundColor();
                }
            }
        }

        /// <summary>
        /// Get all controls recursively
        /// </summary>
        private static System.Collections.Generic.List<Control> GetAllControls(Control parent)
        {
            var controls = new System.Collections.Generic.List<Control>();
            foreach (Control control in parent.Controls)
            {
                controls.Add(control);
                controls.AddRange(GetAllControls(control));
            }
            return controls;
        }

        /// <summary>
        /// Create modern button style
        /// </summary>
        public static void StyleButton(Button button, bool isPrimary = true)
        {
            button.BackColor = isPrimary ? Colors.AccentColor : GetSurfaceColor();
            button.ForeColor = isPrimary ? Color.White : GetForegroundColor();
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = Fonts.NormalFont;
            button.Height = 36;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Create modern panel style
        /// </summary>
        public static void StylePanel(Panel panel)
        {
            panel.BackColor = GetSurfaceColor();
            panel.BorderStyle = BorderStyle.None;
        }

        /// <summary>
        /// Create status color based on message type
        /// </summary>
        public static Color GetStatusColor(StatusType type)
        {
            return type switch
            {
                StatusType.Success => Colors.Success,
                StatusType.Warning => Colors.Warning,
                StatusType.Error => Colors.Error,
                StatusType.Info => Colors.Info,
                _ => Colors.Neutral
            };
        }

        public enum StatusType
        {
            Success,
            Warning,
            Error,
            Info,
            Neutral
        }
    }
}
