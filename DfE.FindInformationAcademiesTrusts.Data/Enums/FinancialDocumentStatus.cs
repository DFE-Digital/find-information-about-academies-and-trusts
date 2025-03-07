namespace DfE.FindInformationAcademiesTrusts.Data.Enums;

public enum FinancialDocumentStatus
{
    /// <summary>
    /// There is a link to financial document
    /// </summary>
    Submitted,

    /// <summary>
    /// There is no link present to the relevant financial document in the latest (current) financial year
    /// </summary>
    NotYetSubmitted,

    /// <summary>
    /// There is no link present to the relevant financial document in a previous financial year.
    /// </summary>
    NotSubmitted,

    /// <summary>
    /// The trust did not open in the financial year and therefore financial documents were not expected.
    /// </summary>
    NotExpected,

    /// <summary>
    /// There was an issue retrieving information about this financial document
    /// </summary>
    Error
}
