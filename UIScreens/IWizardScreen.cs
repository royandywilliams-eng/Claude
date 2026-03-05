using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Interface for all wizard screens
    /// Ensures consistent behavior across all screens
    /// </summary>
    public interface IWizardScreen
    {
        /// <summary>
        /// Get the screen control to display
        /// </summary>
        Control GetScreenControl();

        /// <summary>
        /// Validate the current screen's data
        /// </summary>
        bool ValidateScreen();

        /// <summary>
        /// Get validation error message if validation fails
        /// </summary>
        string GetValidationError();

        /// <summary>
        /// Called when screen is loaded
        /// </summary>
        void OnLoad();

        /// <summary>
        /// Called when screen is about to be hidden
        /// </summary>
        void OnUnload();
    }
}
