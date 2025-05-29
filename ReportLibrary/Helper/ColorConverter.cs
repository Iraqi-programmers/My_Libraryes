using QuestPDF.Infrastructure;
using System.Drawing;

namespace ReportLibrary.Helper
{
    public static class ColorConverter
    {
        /// <summary>
        /// Converts System.Drawing.Color to QuestPDF compatible color.
        /// </summary>
        public static QuestPDF.Infrastructure.Color ToQuestColor(System.Drawing.Color color)
        {
            return QuestPDF.Infrastructure.Color.FromRGB(color.R, color.G, color.B);
        }
    }
}
