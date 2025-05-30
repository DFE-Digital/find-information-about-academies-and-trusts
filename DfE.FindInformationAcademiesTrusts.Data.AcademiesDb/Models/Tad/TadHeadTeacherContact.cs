using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;

[ExcludeFromCodeCoverage] // Database model POCO
public class TadHeadTeacherContact
{
    public int Urn { get; set; }

    public string? HeadFirstName { get; set; }

    public string? HeadLastName { get; set; }

    public string? HeadEmail { get; set; }

    public string? FileName { get; set; }

    public DateTime? DateImported { get; set; }
}
