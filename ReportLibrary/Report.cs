// ✅ Combined export-oriented report builder (PDF via QuestPDF, Excel via ClosedXML)
using ReportLibrary.Helper;
using System.Data;
using System.Diagnostics;

namespace ReportLibrary
{
    /// <summary>
    /// Provides functionality to export tabular data to PDF or Excel using QuestPDF and ClosedXML.
    /// </summary>
    public static class Report
    {
        public static string? LastErrorMessage { get; private set; }

        #region Exports
        /// <summary>
        /// Exports the given DataTable to the specified format (.pdf or .xlsx).
        /// </summary>
        public static bool Export(DataTable data, ReportOptions options, string outputPath)
        {
            try
            {
                string extension = Path.GetExtension(outputPath).ToLower();
                return extension switch
                {
                    ".pdf" => PdfReportBuilder.GeneratePdfReport(data, options, outputPath),
                    ".xlsx" => ExcelReportBuilder.GenerateExcelReport(data, options, outputPath),
                    _ => throw new NotSupportedException("Only .pdf and .xlsx formats are supported.")
                };
            }
            catch (Exception ex) 
            {
                LastErrorMessage= ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Overload: Exports IEnumerable<T/> to the specified format (.pdf or .xlsx).
        /// </summary>
        public static bool Export<T>(IEnumerable<T> data, ReportOptions options, string outputPath)
        {
            try
            {
                var table = ReportHelper.ToDataTable(data);
                return Export(table, options, outputPath);
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Print
        /// <summary>
        /// Print Only PDF reports.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool Print(DataTable data, ReportOptions options)
        {
            // Generate temp PDF file
            string tempPath = Path.Combine(Path.GetTempPath(), $"Report_{Guid.NewGuid()}.pdf");
            bool success = PdfReportBuilder.GeneratePdfReport(data, options, tempPath);

            if (success)
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = tempPath,
                            Verb = "Print",
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        }
                    };
                    process.Start();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }


        /// <summary>
        /// Overload: Print PDF report from IEnumerable<T/>.
        /// </summary>
        public static bool Print<T>(IEnumerable<T> data, ReportOptions options)
        {
            try
            {
                var table = ReportHelper.ToDataTable(data);
                return Print(table, options);
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                return false;
            }
        }
        #endregion

    }
}
