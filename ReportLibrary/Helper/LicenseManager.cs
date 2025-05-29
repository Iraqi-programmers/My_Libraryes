using QuestPDF.Infrastructure;

namespace ReportLibrary.Helper
{
    /// <summary>
    /// Ensures required license setup and initialization for the reporting system.
    /// </summary>
    internal static class LicenseManager
    {
        private static bool _initialized = false;

        public static void EnsureLicense()
        {
            if (!_initialized)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                _initialized = true;
            }
        }
    }
}
