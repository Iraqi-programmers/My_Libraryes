namespace ReportLibrary.Enums
{
    /// <summary>
    /// Defines the type of report to generate.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// A report for sales transactions.
        /// </summary>
        Sales,

        /// <summary>
        /// A report for inventory or stock levels.
        /// </summary>
        Inventory,

        /// <summary>
        /// A custom report defined by the user.
        /// </summary>
        Custom,
        Financial,
        Purchase,
        CustomerList

    }
}
