#!/usr/bin/env python3
"""
NT Q&A Application Setup Wizard
Guided installation and configuration for Windows
"""

import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
import json
import subprocess
import shutil
import os
import sys
from pathlib import Path
from datetime import datetime
import winreg

class NTQASetupWizard(tk.Tk):
    def __init__(self, app_directory):
        super().__init__()
        self.app_dir = Path(app_directory)
        self.setup_dir = self.app_dir / 'setup'
        self.title("NT Q&A Application Setup")
        self.geometry("650x550")
        self.resizable(False, False)

        # Center window on screen
        self.update_idletasks()
        width = self.winfo_width()
        height = self.winfo_height()
        x = (self.winfo_screenwidth() // 2) - (width // 2)
        y = (self.winfo_screenheight() // 2) - (height // 2)
        self.geometry(f'+{x}+{y}')

        # Apply modern theme
        style = ttk.Style()
        style.theme_use('clam')

        # Configuration data
        self.config_data = {
            'setup_date': datetime.now().isoformat(),
            'app_port': 3000,
            'openai_api_key': '',
            'theme': 'light',
            'shortcuts_created': False,
            'node_version': 'unknown'
        }

        # Setup steps
        self.steps = [
            'welcome',
            'system_check',
            'configuration',
            'installation',
            'shortcuts',
            'quick_start',
            'finish'
        ]
        self.current_step = 0

        # Main frame
        self.main_frame = ttk.Frame(self)
        self.main_frame.pack(fill=tk.BOTH, expand=True, padx=20, pady=20)

        # Show first step
        self.show_step()

    def clear_frame(self):
        """Clear all widgets from main frame"""
        for widget in self.main_frame.winfo_children():
            widget.destroy()

    def show_step(self):
        """Display current step"""
        step_name = self.steps[self.current_step]
        method_name = f'show_{step_name}'
        getattr(self, method_name)()

    def show_welcome(self):
        """Step 1: Welcome"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Welcome to NT Q&A Setup",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        subtitle = ttk.Label(self.main_frame,
                            text="New Testament Q&A Application",
                            font=('Arial', 10))
        subtitle.pack(pady=5)

        description = ttk.Label(self.main_frame,
                               text="This wizard will guide you through setting up the\n"
                                    "NT Q&A Application on your computer.",
                               justify=tk.CENTER,
                               font=('Arial', 10))
        description.pack(pady=15)

        reqs_title = ttk.Label(self.main_frame,
                              text="System Requirements:",
                              font=('Arial', 10, 'bold'))
        reqs_title.pack(pady=5)

        reqs_text = ttk.Label(self.main_frame,
                             text="• Node.js v14.0 or higher\n"
                                  "• npm (included with Node.js)\n"
                                  "• ~100 MB disk space\n"
                                  "• Windows 7 or later",
                             justify=tk.LEFT,
                             font=('Arial', 9))
        reqs_text.pack(pady=5, anchor='w')

        # Navigation
        self.show_navigation_buttons(show_back=False, back_text="Back", next_text="Next")

    def show_system_check(self):
        """Step 2: System Check"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="System Check",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        status = ttk.Label(self.main_frame,
                          text="Checking your system...",
                          font=('Arial', 10))
        status.pack(pady=15)

        # Check Node.js
        node_version = self.check_nodejs()
        self.config_data['node_version'] = node_version

        # Display results
        if node_version and node_version != 'not_found':
            result_text = f"✓ Node.js {node_version} is installed"
            result_color = 'green'
        else:
            result_text = "✗ Node.js is not installed"
            result_color = 'red'

        result = ttk.Label(self.main_frame,
                          text=result_text,
                          font=('Arial', 11, 'bold'),
                          foreground=result_color)
        result.pack(pady=20)

        if node_version == 'not_found':
            warning = ttk.Label(self.main_frame,
                               text="Node.js is required to run the NT Q&A App.\n\n"
                                    "Visit https://nodejs.org (LTS version)\n"
                                    "and install Node.js, then run this wizard again.",
                               justify=tk.CENTER,
                               font=('Arial', 9),
                               foreground='red')
            warning.pack(pady=10)
            self.show_navigation_buttons(show_back=True, back_text="Back", next_text="Exit")
        else:
            self.show_navigation_buttons(show_back=True, back_text="Back", next_text="Next")

    def check_nodejs(self):
        """Check if Node.js is installed and get version"""
        try:
            result = subprocess.run(['node', '--version'],
                                  capture_output=True,
                                  text=True,
                                  timeout=5)
            if result.returncode == 0:
                version = result.stdout.strip().lstrip('v')
                return version
        except (FileNotFoundError, subprocess.TimeoutExpired):
            pass
        return 'not_found'

    def show_configuration(self):
        """Step 3: Configuration"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Configuration",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        subtitle = ttk.Label(self.main_frame,
                            text="Choose your preferences",
                            font=('Arial', 10))
        subtitle.pack(pady=5)

        # Port selection
        port_frame = ttk.LabelFrame(self.main_frame, text="Web Server Port", padding=10)
        port_frame.pack(fill=tk.X, pady=10)

        ttk.Label(port_frame, text="Port number (default 3000):").pack(anchor='w')
        self.port_var = tk.StringVar(value="3000")
        port_spin = ttk.Spinbox(port_frame, from_=1024, to=65535,
                               textvariable=self.port_var, width=20)
        port_spin.pack(anchor='w', pady=5)
        ttk.Label(port_frame, text="(Most users should keep default)",
                 font=('Arial', 8), foreground='gray').pack(anchor='w')

        # Theme selection
        theme_frame = ttk.LabelFrame(self.main_frame, text="Theme", padding=10)
        theme_frame.pack(fill=tk.X, pady=10)

        self.theme_var = tk.StringVar(value="light")
        ttk.Radiobutton(theme_frame, text="Light Theme",
                       variable=self.theme_var, value="light").pack(anchor='w')
        ttk.Radiobutton(theme_frame, text="Dark Theme",
                       variable=self.theme_var, value="dark").pack(anchor='w')

        # OpenAI API Key (optional)
        api_frame = ttk.LabelFrame(self.main_frame, text="Optional: OpenAI API Key", padding=10)
        api_frame.pack(fill=tk.X, pady=10)

        ttk.Label(api_frame, text="For AI summarization (leave empty to skip):",
                 font=('Arial', 9)).pack(anchor='w')
        self.api_var = tk.StringVar()
        api_entry = ttk.Entry(api_frame, textvariable=self.api_var, width=40, show="*")
        api_entry.pack(anchor='w', pady=5)

        self.show_navigation_buttons(show_back=True, back_text="Back", next_text="Next")

    def show_installation(self):
        """Step 4: Installation"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Installation",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        status_label = ttk.Label(self.main_frame,
                                text="Installing dependencies...",
                                font=('Arial', 10))
        status_label.pack(pady=15)

        # Progress bar
        progress = ttk.Progressbar(self.main_frame, length=300, mode='indeterminate')
        progress.pack(pady=20)
        progress.start()

        # Log display
        log_frame = ttk.Frame(self.main_frame)
        log_frame.pack(fill=tk.BOTH, expand=True, pady=10)

        log_text = scrolledtext.ScrolledText(log_frame, height=8, width=60,
                                            state=tk.DISABLED, font=('Courier', 8))
        log_text.pack(fill=tk.BOTH, expand=True)

        # Update UI
        self.update()

        # Run npm install
        try:
            self.update_log(log_text, "Starting npm install...")
            result = subprocess.run(
                ['npm', 'install'],
                cwd=str(self.app_dir),
                capture_output=True,
                text=True,
                timeout=300
            )

            if result.returncode == 0:
                self.update_log(log_text, "✓ Dependencies installed successfully!")
                self.update_log(log_text, "Setup can continue.")
                status_label.config(text="Installation Complete", foreground='green')
            else:
                self.update_log(log_text, f"✗ Installation failed: {result.stderr}")
                status_label.config(text="Installation Failed", foreground='red')
        except Exception as e:
            self.update_log(log_text, f"✗ Error: {str(e)}")
            status_label.config(text="Installation Error", foreground='red')

        progress.stop()

        # Save configuration
        self.config_data['app_port'] = int(self.port_var.get())
        self.config_data['theme'] = self.theme_var.get()
        self.config_data['openai_api_key'] = self.api_var.get()

        self.show_navigation_buttons(show_back=False, back_text="Back", next_text="Next")

    def update_log(self, text_widget, message):
        """Update log display"""
        text_widget.config(state=tk.NORMAL)
        text_widget.insert(tk.END, message + '\n')
        text_widget.see(tk.END)
        text_widget.config(state=tk.DISABLED)
        self.update()

    def show_shortcuts(self):
        """Step 5: Shortcuts Setup"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Create Shortcuts",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        subtitle = ttk.Label(self.main_frame,
                            text="Select where to create shortcuts",
                            font=('Arial', 10))
        subtitle.pack(pady=5)

        # Checkbox frame
        check_frame = ttk.Frame(self.main_frame)
        check_frame.pack(fill=tk.X, pady=20)

        self.desktop_var = tk.BooleanVar(value=True)
        self.startmenu_var = tk.BooleanVar(value=True)

        ttk.Checkbutton(check_frame, text="Create Desktop shortcut",
                       variable=self.desktop_var).pack(anchor='w', pady=10)
        ttk.Checkbutton(check_frame, text="Create Start Menu shortcut",
                       variable=self.startmenu_var).pack(anchor='w', pady=10)

        info = ttk.Label(self.main_frame,
                        text="Shortcuts will allow you to easily launch the NT Q&A App",
                        font=('Arial', 9),
                        foreground='gray')
        info.pack(pady=10)

        self.show_navigation_buttons(show_back=True, back_text="Back", next_text="Next")

    def show_quick_start(self):
        """Step 6: Quick Start Guide"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Quick Start Guide",
                         font=('Arial', 16, 'bold'))
        title.pack(pady=10)

        guide_text = """
How to use the NT Q&A Application:

1. LAUNCHING THE APP
   • Click the desktop shortcut or Start Menu entry
   • The app will open at http://localhost:3000

2. BROWSING QUESTIONS
   • Use the "Browse Books" tab to view books and chapters
   • Select a book to see all Q&A pairs

3. SEARCHING
   • Use the "Search" tab to find specific questions
   • Search works across all books and answers

4. RANDOM QUESTIONS
   • Click "Get Random Question" to test your knowledge

5. EXPORTING DATA
   • Select questions and export them as text files
   • Generate summaries of selected questions

6. SETTINGS
   • API Key: Add your OpenAI key for AI-powered summaries
   • Port: The app runs on port 3000 (configurable)

For more help, visit the documentation or check the README file.
        """

        guide_display = scrolledtext.ScrolledText(self.main_frame, height=12,
                                                  width=60, font=('Arial', 9),
                                                  state=tk.DISABLED)
        guide_display.pack(fill=tk.BOTH, expand=True, pady=10)
        guide_display.config(state=tk.NORMAL)
        guide_display.insert(1.0, guide_text.strip())
        guide_display.config(state=tk.DISABLED)

        self.show_navigation_buttons(show_back=True, back_text="Back", next_text="Next")

    def show_finish(self):
        """Step 7: Finish"""
        self.clear_frame()

        title = ttk.Label(self.main_frame, text="Setup Complete!",
                         font=('Arial', 16, 'bold'), foreground='green')
        title.pack(pady=10)

        message = ttk.Label(self.main_frame,
                           text="NT Q&A Application has been successfully installed!",
                           font=('Arial', 11),
                           justify=tk.CENTER)
        message.pack(pady=15)

        # Create shortcuts
        self.create_shortcuts()

        # Save configuration
        self.save_config()

        summary = f"""
Setup Summary:
• Node.js Version: {self.config_data['node_version']}
• Port: {self.config_data['app_port']}
• Theme: {self.config_data['theme'].capitalize()}
• Shortcuts Created: {'Yes' if self.config_data['shortcuts_created'] else 'No'}

You can now launch the application using the shortcuts,
or run 'npm start' in the app directory.

Enjoy the NT Q&A Application!
        """

        summary_display = ttk.Label(self.main_frame,
                                   text=summary.strip(),
                                   font=('Courier', 9),
                                   justify=tk.LEFT,
                                   background='#f0f0f0',
                                   relief=tk.SUNKEN,
                                   padding=10)
        summary_display.pack(fill=tk.BOTH, expand=True, padx=10, pady=10)

        # Finish button
        button_frame = ttk.Frame(self.main_frame)
        button_frame.pack(fill=tk.X, pady=20)

        finish_btn = ttk.Button(button_frame, text="Finish",
                               command=self.quit)
        finish_btn.pack(side=tk.RIGHT, padx=5)

    def create_shortcuts(self):
        """Create Windows shortcuts"""
        try:
            import win32com.client
            shell = win32com.client.Dispatch("WScript.Shell")

            # Create startup batch file
            startup_bat = self.app_dir / 'NT-QA-App.bat'
            if not startup_bat.exists():
                self.create_startup_script(startup_bat)

            # Desktop shortcut
            if self.desktop_var.get():
                desktop = Path.home() / 'Desktop' / 'NT-QA-App.lnk'
                shortcut = shell.CreateShortCut(str(desktop))
                shortcut.Targetpath = str(startup_bat)
                shortcut.WorkingDirectory = str(self.app_dir)
                shortcut.Description = "NT Q&A Application"
                shortcut.save()

            # Start Menu shortcut
            if self.startmenu_var.get():
                start_menu = Path.home() / 'AppData' / 'Roaming' / 'Microsoft' / 'Windows' / 'Start Menu' / 'Programs' / 'NT-QA-App.lnk'
                start_menu.parent.mkdir(parents=True, exist_ok=True)
                shortcut = shell.CreateShortCut(str(start_menu))
                shortcut.Targetpath = str(startup_bat)
                shortcut.WorkingDirectory = str(self.app_dir)
                shortcut.Description = "NT Q&A Application"
                shortcut.save()

            self.config_data['shortcuts_created'] = True
        except ImportError:
            messagebox.showwarning("Warning",
                                  "Could not create shortcuts (pywin32 not installed).\n"
                                  "You can start the app by running NT-QA-App.bat manually.")
            self.config_data['shortcuts_created'] = False
        except Exception as e:
            messagebox.showerror("Error", f"Failed to create shortcuts: {str(e)}")

    def create_startup_script(self, bat_file):
        """Create startup batch script"""
        script = f"""@echo off
cd /d "{self.app_dir}"
if not exist node_modules (
    echo Installing dependencies...
    call npm install
)
echo Starting NT Q&A App on port {self.config_data['app_port']}...
start http://localhost:{self.config_data['app_port']}
call npm start
pause
"""
        with open(bat_file, 'w') as f:
            f.write(script)

    def save_config(self):
        """Save configuration to file"""
        config_dir = Path.home() / '.nt-qa-app'
        config_dir.mkdir(exist_ok=True)

        config_file = config_dir / 'setup_config.json'
        with open(config_file, 'w') as f:
            json.dump(self.config_data, f, indent=2)

    def show_navigation_buttons(self, show_back=True, back_text="Back", next_text="Next"):
        """Display navigation buttons"""
        button_frame = ttk.Frame(self.main_frame)
        button_frame.pack(fill=tk.X, pady=20)

        if show_back:
            back_btn = ttk.Button(button_frame, text=back_text,
                                 command=self.prev_step)
            back_btn.pack(side=tk.LEFT, padx=5)

        next_btn = ttk.Button(button_frame, text=next_text,
                             command=self.next_step)
        next_btn.pack(side=tk.RIGHT, padx=5)

    def next_step(self):
        """Move to next step with validation"""
        step_name = self.steps[self.current_step]

        # Validation
        if step_name == 'system_check':
            if self.config_data['node_version'] == 'not_found':
                self.quit()
                return
        elif step_name == 'configuration':
            try:
                port = int(self.port_var.get())
                if port < 1024 or port > 65535:
                    messagebox.showerror("Invalid Port",
                                        "Port must be between 1024 and 65535")
                    return
            except ValueError:
                messagebox.showerror("Invalid Port", "Port must be a number")
                return

        if self.current_step < len(self.steps) - 1:
            self.current_step += 1
            self.show_step()

    def prev_step(self):
        """Move to previous step"""
        if self.current_step > 0:
            self.current_step -= 1
            self.show_step()


if __name__ == '__main__':
    # Get app directory from command line argument or use current directory
    if len(sys.argv) > 1:
        app_dir = Path(sys.argv[1])
    else:
        app_dir = Path(__file__).parent.parent

    app = NTQASetupWizard(app_dir)
    app.mainloop()
