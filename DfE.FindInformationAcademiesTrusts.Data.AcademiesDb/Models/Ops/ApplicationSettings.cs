namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;

public class ApplicationSettings
{
    public required string Key { get; set; }
    public string? Value { get; set; }
    public DateTime? Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
