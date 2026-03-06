using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Interface for all configuration tabs
    /// Ensures consistent behavior across all tabs
    /// </summary>
    public interface IConfigurationTab
    {
        /// <summary>
        /// Get the tab control to display
        /// </summary>
        Control GetTabControl();

        /// <summary>
        /// Validate the tab's data
        /// </summary>
        bool ValidateTab();

        /// <summary>
        /// Get validation error message if validation fails
        /// </summary>
        string GetValidationError();

        /// <summary>
        /// Called when tab is loaded
        /// </summary>
        void OnLoad();

        /// <summary>
        /// Called when tab is about to be unloaded
        /// </summary>
        void OnUnload();
    }
}
