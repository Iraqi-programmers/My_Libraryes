// ✅ Updated ExcelReportBuilder to support extended ReportOptions
using ClosedXML.Excel;
using ReportLibrary.Helper;
using System;
using System.Data;
using System.IO;

namespace ReportLibrary
{
    /// <summary>
    /// Provides functionality to generate Excel (.xlsx) reports from a DataTable using ClosedXML.
    /// </summary>
    internal  class ExcelReportBuilder
    {
        /// <summary>
        /// Generates an Excel report and saves it to the specified output path.
        /// </summary>
        /// <param name="data">The source data as a DataTable.</param>
        /// <param name="options">The report customization options (title, footer, font size, etc.).</param>
        /// <param name="outputPath">The full path where the Excel file will be saved.</param>
        /// <exception cref="ArgumentException">Thrown if the provided DataTable is null or empty.</exception>
        internal static bool GenerateExcelReport(DataTable data, ReportOptions options, string outputPath)
        {
            try
            {
                if (data == null || data.Rows.Count == 0)
                    return false;

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Report");
                    int currentRow = 1;

                    var visibleColumns = data.Columns.Cast<DataColumn>()
                        .Where(c => !options.ExcludedColumns.Contains(c.ColumnName))
                        .ToList();

                    if (options.IsRightToLeft)
                        visibleColumns.Reverse();

                    // ✅ عنوان التقرير
                    worksheet.Cell(currentRow, 1).Value = options.ReportTitle;
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 1).Style.Font.FontSize = options.FontSize + 2;
                    worksheet.Cell(currentRow, 1).Style.Font.FontName = options.FontFamily;
                    worksheet.Range(currentRow, 1, currentRow, visibleColumns.Count)
                             .Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow += 2;

                    // ✅ رؤوس الأعمدة
                    int colIndex = 1;
                    foreach (var col in visibleColumns)
                    {
                        string displayName = options.ColumnDisplayNames.ContainsKey(col.ColumnName)
                            ? options.ColumnDisplayNames[col.ColumnName]
                            : col.ColumnName;

                        var headerCell = worksheet.Cell(currentRow, colIndex);
                        headerCell.Value = displayName;
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.FontName = options.FontFamily;
                        headerCell.Style.Fill.BackgroundColor = XLColor.FromColor(options.HeaderBackgroundColor);
                        colIndex++;
                    }
                    currentRow++;

                    // ✅ البيانات
                    bool isStriped = false;
                    foreach (DataRow row in data.Rows)
                    {
                        int cellIndex = 1;
                        isStriped = options.UseZebraStriping ? !isStriped : false;

                        foreach (var col in visibleColumns)
                        {
                            var cell = worksheet.Cell(currentRow, cellIndex);
                            cell.Value = row[col];
                            cell.Style.Font.FontSize = options.FontSize;
                            cell.Style.Font.FontName = options.FontFamily;
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cell.Style.Border.OutsideBorderColor = XLColor.FromColor(options.BorderColor);

                            if (options.ExColumnAlignment.TryGetValue(col.ColumnName, out string? align))
                            {
                                if (Enum.TryParse(align, out XLAlignmentHorizontalValues alignment))
                                    cell.Style.Alignment.Horizontal = alignment;
                            }

                            if (isStriped)
                                cell.Style.Fill.BackgroundColor = XLColor.LightGray;

                            cellIndex++;
                        }
                        currentRow++;
                    }

                    // ✅ الصف الإجمالي
                    if (options.SummaryColumns != null && options.SummaryColumns.Any())
                    {
                        int cellIndex = 1;
                        foreach (var col in visibleColumns)
                        {
                            var cell = worksheet.Cell(currentRow, cellIndex);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.FontName = options.FontFamily;
                            cell.Style.Fill.BackgroundColor = XLColor.FromColor(options.HeaderBackgroundColor);
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cell.Style.Border.OutsideBorderColor = XLColor.FromColor(options.BorderColor);

                            if (options.SummaryColumns.Contains(col.ColumnName))
                            {
                                decimal sum = 0;
                                foreach (DataRow row in data.Rows)
                                {
                                    if (decimal.TryParse(row[col.ColumnName]?.ToString()?.Replace(",", ""), out var val))
                                        sum += val;
                                }

                                var label = options.SummaryLabels != null && options.SummaryLabels.TryGetValue(col.ColumnName, out var lbl) ? lbl : "Total";
                                cell.Value = $"{label}: {sum:N2}";
                            }
                            else
                            {
                                cell.Value = "";
                            }

                            cellIndex++;
                        }
                        currentRow++;
                    }

                    // ✅ الفوتر
                    worksheet.Cell(currentRow, 1).Value = options.FooterText;
                    worksheet.Cell(currentRow, 1).Style.Font.Italic = true;
                    worksheet.Cell(currentRow, 1).Style.Font.FontName = options.FontFamily;
                    worksheet.Range(currentRow, 1, currentRow, visibleColumns.Count).Merge()
                             .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    // ✅ تقرير ID
                    if (!string.IsNullOrWhiteSpace(options.ReportId))
                    {
                        worksheet.Cell(currentRow, 1).Value = $"Report ID: {options.ReportId}";
                        worksheet.Cell(currentRow, 1).Style.Font.FontName = options.FontFamily;
                        worksheet.Range(currentRow, 1, currentRow, visibleColumns.Count).Merge()
                                 .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        currentRow++;
                    }

                    // ✅ التاريخ والوقت
                    if (options.ShowDateTime)
                    {
                        worksheet.Cell(currentRow, 1).Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}";
                        worksheet.Cell(currentRow, 1).Style.Font.FontName = options.FontFamily;
                        worksheet.Range(currentRow, 1, currentRow, visibleColumns.Count).Merge()
                                 .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        currentRow++;
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(outputPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطأ أثناء توليد تقرير Excel: " + ex.Message);
                return false;
            }
        }
    }
}
