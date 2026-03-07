using System;
using System.Drawing;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Manages system tray integration
    /// </summary>
    public class TrayIconManager : IDisposable
    {
        private NotifyIcon? _trayIcon;
        private ContextMenuStrip? _contextMenu;
        private Form? _mainForm;

        public event EventHandler? ShowWindowRequested;
        public event EventHandler? HideWindowRequested;
        public event EventHandler? StartServerRequested;
        public event EventHandler? StopServerRequested;
        public event EventHandler? ExitRequested;

        public TrayIconManager(Form mainForm)
        {
            _mainForm = mainForm;
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            // Create context menu
            _contextMenu = new ContextMenuStrip();

            var showItem = new ToolStripMenuItem("Show", null, OnShow);
            var hideItem = new ToolStripMenuItem("Hide", null, OnHide);
            _contextMenu.Items.Add(showItem);
            _contextMenu.Items.Add(hideItem);
            _contextMenu.Items.Add(new ToolStripSeparator());

            var startItem = new ToolStripMenuItem("Start Server", null, OnStartServer);
            var stopItem = new ToolStripMenuItem("Stop Server", null, OnStopServer);
            _contextMenu.Items.Add(startItem);
            _contextMenu.Items.Add(stopItem);
            _contextMenu.Items.Add(new ToolStripSeparator());

            var exitItem = new ToolStripMenuItem("Exit", null, OnExit);
            _contextMenu.Items.Add(exitItem);

            // Create tray icon
            _trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "NT Q&A App Launcher",
                ContextMenuStrip = _contextMenu
            };

            _trayIcon.MouseDoubleClick += OnTrayIconDoubleClick;
        }

        /// <summary>
        /// Update tray icon based on server status
        /// </summary>
        public void UpdateStatus(bool isRunning)
        {
            if (_trayIcon == null) return;

            _trayIcon.Text = isRunning
                ? "NT Q&A App Launcher - Running"
                : "NT Q&A App Launcher - Stopped";

            // Change icon color based on status
            _trayIcon.Icon = isRunning ? CreateRunningIcon() : CreateStoppedIcon();
        }

        private Icon CreateRunningIcon()
        {
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.LimeGreen))
                {
                    g.FillEllipse(brush, 2, 2, 12, 12);
                }
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private Icon CreateStoppedIcon()
        {
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.Red))
                {
                    g.FillEllipse(brush, 2, 2, 12, 12);
                }
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }

        public void ShowNotification(string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
        {
            _trayIcon?.ShowBalloonTip(3000, title, text, icon);
        }

        private void OnTrayIconDoubleClick(object? sender, MouseEventArgs e)
        {
            if (_mainForm?.Visible == true)
            {
                HideWindowRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowWindowRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnShow(object? sender, EventArgs e)
        {
            ShowWindowRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnHide(object? sender, EventArgs e)
        {
            HideWindowRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnStartServer(object? sender, EventArgs e)
        {
            StartServerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopServer(object? sender, EventArgs e)
        {
            StopServerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnExit(object? sender, EventArgs e)
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _trayIcon?.Dispose();
            _contextMenu?.Dispose();
        }
    }
}
