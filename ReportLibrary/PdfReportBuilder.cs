using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReportLibrary.Helper;
using System.Data;
using QuestAlign = QuestPDF.Infrastructure.HorizontalAlignment;


namespace ReportLibrary
{
    /// <summary>
    /// Provides functionality to generate PDF reports using QuestPDF from a DataTable.
    /// </summary>
    internal  class PdfReportBuilder
    {
        /// <summary>
        /// Generates a PDF report and saves it to the specified path using the provided data and options.
        /// <param name="data">The source data as a DataTable.</param>
        /// <param name="options">The report customization options (header, footer, font size, etc.).</param>
        /// <param name="outputPath">The full path where the PDF file will be saved.</param>
        /// </summary>
        internal static bool GeneratePdfReport(DataTable data, ReportOptions options, string outputPath)
        {
            try
            {
                LicenseManager.EnsureLicense();

                if (data == null || data.Rows.Count == 0)
                    return false;

                var visibleColumns = data.Columns.Cast<DataColumn>()
                    .Where(c => !options.ExcludedColumns.Contains(c.ColumnName))
                    .ToList();

                if (options.IsRightToLeft)
                    visibleColumns.Reverse();

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(options.PageMargin);
                        page.Size(options.IsLandscape ? PageSizes.A4.Landscape() : PageSizes.A4);

                        page.Header().Element(BuildHeader);
                        page.Content().Element(BuildTable);
                        page.Footer().Element(BuildFooter);

                        void BuildHeader(IContainer container)
                        {
                            container.PaddingBottom(10).Column(col =>
                            {
                                if (options.HeaderImageBytes != null)
                                {
                                    try
                                    {
                                        var image = QuestPDF.Infrastructure.Image.FromStream(new MemoryStream(options.HeaderImageBytes));
                                        var height = options.HeaderImageHeight ?? 60f;

                                        switch (options.HeaderImageAlignment)
                                        {
                                            case QuestAlign.Left:
                                                col.Item().AlignLeft().Height(height).Image(image).FitWidth();
                                                break;
                                            case QuestAlign.Center:
                                                col.Item().AlignCenter().Height(height).Image(image).FitWidth();
                                                break;
                                            case QuestAlign.Right:
                                                col.Item().AlignRight().Height(height).Image(image).FitWidth();
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        col.Item().Text($"⚠ Header image error: {ex.Message}")
                                              .FontColor(Colors.Red.Medium).FontSize(8);
                                    }
                                }

                                var title = col.Item().Text(options.ReportTitle)
                                    .FontSize(options.FontSize + 2)
                                    .FontFamily(options.FontFamily)
                                    .Bold();

                                switch (options.HeaderAlignment)
                                {
                                    case QuestAlign.Left: title.AlignLeft(); break;
                                    case QuestAlign.Center: title.AlignCenter(); break;
                                    case QuestAlign.Right: title.AlignRight(); break;
                                }
                            });
                        }

                        void BuildTable(IContainer container)
                        {
                            var tableBuilder = new Action<TableDescriptor>(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    foreach (var _ in visibleColumns)
                                        columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    foreach (var col in visibleColumns)
                                    {
                                        var display = options.ColumnDisplayNames.TryGetValue(col.ColumnName, out var name) ? name : col.ColumnName;

                                        var cell = header.Cell()
                                            .Background(ColorConverter.ToQuestColor(options.HeaderBackgroundColor))
                                            .Padding(5);

                                        var align = options.ColumnAlignment.TryGetValue(col.ColumnName, out var colAlign)
                                            ? colAlign
                                            : options.HeaderAlignment;

                                        switch (align)
                                        {
                                            case QuestAlign.Left: cell = cell.AlignLeft(); break;
                                            case QuestAlign.Center: cell = cell.AlignCenter(); break;
                                            case QuestAlign.Right: cell = cell.AlignRight(); break;
                                        }

                                        cell.Text(display)
                                            .Bold()
                                            .FontSize(options.FontSize)
                                            .FontFamily(options.FontFamily);
                                    }
                                });

                                bool zebra = false;
                                foreach (DataRow row in data.Rows)
                                {
                                    zebra = options.UseZebraStriping ? !zebra : false;

                                    foreach (var col in visibleColumns)
                                    {
                                        var value = row[col.ColumnName]?.ToString() ?? string.Empty;

                                        var cell = table.Cell()
                                            .Background(zebra ? Colors.Grey.Lighten3 : Colors.White)
                                            .Border(1)
                                            .BorderColor(ColorConverter.ToQuestColor(options.BorderColor))
                                            .Padding(5);

                                        var align = options.ColumnAlignment.TryGetValue(col.ColumnName, out var colAlign)
                                            ? colAlign
                                            : options.CellAlignment;

                                        switch (align)
                                        {
                                            case QuestAlign.Left: cell = cell.AlignLeft(); break;
                                            case QuestAlign.Center: cell = cell.AlignCenter(); break;
                                            case QuestAlign.Right: cell = cell.AlignRight(); break;
                                        }

                                        cell.Text(value)
                                            .FontSize(options.FontSize)
                                            .FontFamily(options.FontFamily);
                                    }
                                }

                                // Summary Row
                                if (options.SummaryColumns != null && options.SummaryColumns.Any())
                                {
                                    foreach (var col in visibleColumns)
                                    {
                                        var cell = table.Cell()
                                            .Background(Colors.Grey.Lighten2)
                                            .Padding(5)
                                            .Border(1)
                                            .BorderColor(ColorConverter.ToQuestColor(options.BorderColor));

                                        var align = options.ColumnAlignment.TryGetValue(col.ColumnName, out var colAlign)
                                            ? colAlign
                                            : options.CellAlignment;

                                        switch (align)
                                        {
                                            case QuestAlign.Left: cell = cell.AlignLeft(); break;
                                            case QuestAlign.Center: cell = cell.AlignCenter(); break;
                                            case QuestAlign.Right: cell = cell.AlignRight(); break;
                                        }

                                        if (options.SummaryColumns.Contains(col.ColumnName))
                                        {
                                            decimal sum = 0;
                                            foreach (DataRow row in data.Rows)
                                            {
                                                var rawValue = row[col.ColumnName]?.ToString()?.Replace(",", "").Replace("د.ع", "").Trim();
                                                if (decimal.TryParse(rawValue, out var val))
                                                    sum += val;

                                            }

                                            var label = options.SummaryLabels != null && options.SummaryLabels.TryGetValue(col.ColumnName, out var lbl) ? lbl : "Total";

                                            cell.Text($"{label}: {sum:N2}")
                                                .Bold()
                                                .FontSize(options.FontSize)
                                                .FontFamily(options.FontFamily);
                                        }
                                        else
                                        {
                                            cell.Text("");
                                        }
                                    }
                                }
                            });

                            if (options.UseTableBorder)
                            {
                                container
                                    .Border(1)
                                    .BorderColor(ColorConverter.ToQuestColor(options.BorderColor))
                                    .Padding(5)
                                    .Table(tableBuilder);
                            }
                            else
                            {
                                container.Table(tableBuilder);
                            }
                        }

                        void BuildFooter(IContainer container)
                        {
                            container.Column(column =>
                            {
                                if (options.FooterImageBytes != null)
                                {
                                    try
                                    {
                                        var image = QuestPDF.Infrastructure.Image.FromStream(new MemoryStream(options.FooterImageBytes));
                                        var height = options.FooterImageHeight ?? 60f;

                                        switch (options.FooterImageAlignment)
                                        {
                                            case QuestAlign.Left:
                                                column.Item().AlignLeft().Height(60).Image(image).FitWidth();
                                                break;
                                            case QuestAlign.Center:
                                                column.Item().AlignCenter().Height(60).Image(image).FitWidth();
                                                break;
                                            case QuestAlign.Right:
                                                column.Item().AlignRight().Height(60).Image(image).FitWidth();
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        column.Item().Text($"⚠ Footer image error: {ex.Message}")
                                              .FontColor(Colors.Red.Medium).FontSize(8);
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(options.FooterText))
                                    column.Item().AlignCenter().Text(options.FooterText).FontFamily(options.FontFamily).Italic().FontSize(10);

                                if (!string.IsNullOrWhiteSpace(options.ReportId))
                                    column.Item().AlignCenter().Text($"Report ID: {options.ReportId}").FontFamily(options.FontFamily).FontSize(8);

                                if (options.ShowDateTime)
                                    column.Item().AlignLeft().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}").FontFamily(options.FontFamily).FontSize(8);

                                if (options.ShowPageNumber)
                                {
                                    column.Item().AlignRight().Text(text =>
                                    {
                                        text.Span("Page ").FontFamily(options.FontFamily).FontSize(8);
                                        text.CurrentPageNumber().FontFamily(options.FontFamily).FontSize(8);
                                        text.Span(" of ").FontFamily(options.FontFamily).FontSize(8);
                                        text.TotalPages().FontFamily(options.FontFamily).FontSize(8);
                                    });
                                }
                            });
                        }
                    });
                });

                document.GeneratePdf(outputPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF report: {ex.Message}");
                return false;
            }
        }

    }
}
