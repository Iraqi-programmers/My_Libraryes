using ReportLibrary.Enums;
using Color = System.Drawing.Color;

namespace ReportLibrary
{
    /// <summary>
    /// Represents the customizable settings for generating a report.
    /// </summary>
    public class ReportOptions
    {
        #region Report identity
        public string ReportTitle { get; set; } = "General Report";
        public ReportType ReportType { get; set; } = ReportType.Custom;
        #endregion

        #region Font and layout
        public float FontSize { get; set; } = 12f;
        public string FontFamily { get; set; } = "Arial";
        public bool IsLandscape { get; set; } = false;
        public int PageMargin { get; set; } = 20;
        #endregion

        #region Header customization
        public byte[]? HeaderImageBytes { get; set; }
        public float? HeaderImageHeight { get; set; } = 60f;
        public QuestPDF.Infrastructure.HorizontalAlignment HeaderImageAlignment { get; set; } = QuestPDF.Infrastructure.HorizontalAlignment.Center;
        public QuestPDF.Infrastructure.HorizontalAlignment? HeaderAlignment { get; set; } = QuestPDF.Infrastructure.HorizontalAlignment.Center;
        #endregion

        #region Footer customization
        public byte[]? FooterImageBytes { get; set; }
        public float? FooterImageHeight { get; set; } = 60f; // in points
        public QuestPDF.Infrastructure.HorizontalAlignment FooterImageAlignment { get; set; } = QuestPDF.Infrastructure.HorizontalAlignment.Center;
        public string FooterText { get; set; } = "Thank you for using our system.";
        #endregion

        #region Border and colors
        public Color HeaderBackgroundColor { get; set; } = Color.LightGray;
        public Color BorderColor { get; set; } = Color.Black;
        public bool UseTableBorder { get; set; } = false;
        #endregion

        #region Alignment
        public QuestPDF.Infrastructure.HorizontalAlignment? CellAlignment { get; set; } = null;
        public Dictionary<string, QuestPDF.Infrastructure.HorizontalAlignment> ColumnAlignment { get; set; } = new();
        public Dictionary<string, string> ExColumnAlignment { get; set; } = new(); // For Excel
        #endregion

        #region Zebra Striping
        /// <summary>
        /// Color for the zebra striping effect on table rows.
        /// </summary>
        public bool UseZebraStriping { get; set; } = true;
        #endregion

        #region Display customization
        public Dictionary<string, string> ColumnDisplayNames { get; set; } = new();
        public List<string> ExcludedColumns { get; set; } = new();
        public bool IsRightToLeft { get; set; } = true;
        #endregion

        #region Footer metadata
        public bool ShowPageNumber { get; set; } = true;
        public bool ShowDateTime { get; set; } = true;
        public string ReportId { get; set; } = string.Empty;

        /// <summary>
        /// اسماء الاعمدة التي تريد حسابها في التقرير , يجب ان تكون الاسماء مطابقة تماما لأسم العمود في شاشة العرض
        /// </summary>
        public List<string> SummaryColumns { get; set; } = new(); // الأعمدة التي تريد حسابها

        /// <summary>
        ///  اسماء الأعمدة الإجمالية (اختياري) - يستخدم في التقارير التي تحتوي على إجماليات
        ///  First String is the column name In DataTable 
        ///  second String is the label to display in the report
        /// </summary>
        public Dictionary<string, string> SummaryLabels { get; set; } = new(); // اسم لكل عمود إجمالي (اختياري)
        #endregion

    }
}
