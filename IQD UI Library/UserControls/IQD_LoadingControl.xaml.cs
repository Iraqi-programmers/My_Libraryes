using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IQD_UI_Library
{
    /// <summary>
    /// Represents a loading window with animated visual elements.
    /// This window is used to indicate that a process is running,
    /// preventing user interaction with the main application until the process completes.
    /// </summary>
    public partial class IQD_LoadingControl : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IQD_LoadingControl"/> class.
        /// </summary>
        public IQD_LoadingControl()
        {
            InitializeComponent();
        }
    }
}

