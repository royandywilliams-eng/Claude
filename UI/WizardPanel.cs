using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UI
{
    /// <summary>
    /// Left panel containing the wizard screens for guided project setup
    /// Handles screen navigation and data binding
    /// </summary>
    public class WizardPanel : Panel
    {
        public event EventHandler ConfigurationChanged;

        private ProjectConfiguration configuration;
        private int currentScreen = 0;
        private Panel screenContainer;
        private Label screenNumberLabel;
        private Label screenTitleLabel;
        private Button previousButton;
        private Button nextButton;
        private Button skipButton;
        private Label descriptionLabel;

        public WizardPanel(ProjectConfiguration config)
        {
            this.configuration = config;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = SystemColors.Control;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(10);

            // Header
            Label headerLabel = new Label
            {
                Text = "📋 Project Setup Wizard",
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(10, 10)
            };
            this.Controls.Add(headerLabel);

            // Screen number indicator
            screenNumberLabel = new Label
            {
                Text = "Step 1 of 6",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = SystemColors.GrayText,
                Location = new Point(10, 35)
            };
            this.Controls.Add(screenNumberLabel);

            // Screen title
            screenTitleLabel = new Label
            {
                Text = "Project Type & Scope",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 55)
            };
            this.Controls.Add(screenTitleLabel);

            // Description
            descriptionLabel = new Label
            {
                Text = "Configure basic project information",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = SystemColors.GrayText,
                Location = new Point(10, 75),
                MaximumSize = new Size(250, 40)
            };
            this.Controls.Add(descriptionLabel);

            // Screen container
            screenContainer = new Panel
            {
                Location = new Point(10, 120),
                Size = new Size(250, 400),
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(screenContainer);

            // Navigation buttons
            previousButton = new Button
            {
                Text = "◄ Previous",
                Location = new Point(10, 530),
                Size = new Size(70, 30),
                Enabled = false
            };
            previousButton.Click += PreviousButton_Click;
            this.Controls.Add(previousButton);

            nextButton = new Button
            {
                Text = "Next ►",
                Location = new Point(90, 530),
                Size = new Size(70, 30)
            };
            nextButton.Click += NextButton_Click;
            this.Controls.Add(nextButton);

            skipButton = new Button
            {
                Text = "Skip",
                Location = new Point(170, 530),
                Size = new Size(50, 30)
            };
            skipButton.Click += SkipButton_Click;
            this.Controls.Add(skipButton);

            // Progress bar
            ProgressBar progressBar = new ProgressBar
            {
                Location = new Point(10, 570),
                Size = new Size(250, 20),
                Value = 17 // 1 of 6 screens
            };
            this.Controls.Add(progressBar);

            LoadScreen(0);
        }

        private void LoadScreen(int screenNumber)
        {
            currentScreen = screenNumber;
            screenContainer.Controls.Clear();

            // Create placeholder for each screen
            Label screenLabel = new Label
            {
                Text = $"Screen {screenNumber + 1} Content",
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = SystemColors.Highlight,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            screenContainer.Controls.Add(screenLabel);

            Label contentLabel = new Label
            {
                Text = $"Screen {screenNumber + 1} will be implemented in Phase 2",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoSize = false,
                TextAlign = ContentAlignment.TopLeft
            };
            screenContainer.Controls.Add(contentLabel);

            // Update header
            screenNumberLabel.Text = $"Step {screenNumber + 1} of 6";
            screenTitleLabel.Text = GetScreenTitle(screenNumber);
            descriptionLabel.Text = GetScreenDescription(screenNumber);

            // Update button states
            previousButton.Enabled = screenNumber > 0;
            nextButton.Enabled = screenNumber < 5;
            skipButton.Enabled = screenNumber < 5;

            OnConfigurationChanged();
        }

        private string GetScreenTitle(int screenNumber)
        {
            return screenNumber switch
            {
                0 => "Project Type & Scope",
                1 => "Technology Stack",
                2 => "Key Features",
                3 => "UI/UX Requirements",
                4 => "Performance & Scalability",
                5 => "Deployment & DevOps",
                _ => "Unknown"
            };
        }

        private string GetScreenDescription(int screenNumber)
        {
            return screenNumber switch
            {
                0 => "Configure your project name, type, and basic information",
                1 => "Choose programming language, frameworks, and database",
                2 => "Select the features you need (authentication, payments, etc.)",
                3 => "Define UI/UX requirements (design framework, responsive, dark mode)",
                4 => "Set performance targets and scalability requirements",
                5 => "Choose hosting platform, CI/CD pipeline, and deployment options",
                _ => "Project setup"
            };
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (currentScreen > 0)
            {
                LoadScreen(currentScreen - 1);
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (currentScreen < 5)
            {
                LoadScreen(currentScreen + 1);
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            // Skip ahead to next screen after this one
            if (currentScreen < 4)
            {
                LoadScreen(currentScreen + 2);
            }
            else if (currentScreen == 4)
            {
                LoadScreen(5);
            }
        }

        public void SetConfiguration(ProjectConfiguration config)
        {
            this.configuration = config;
            LoadScreen(0);
        }

        protected void OnConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
