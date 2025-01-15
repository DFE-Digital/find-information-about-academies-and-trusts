namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy
{
    public record PipelineEstablishment(
    string? Urn,
    string? EstablishmentName,
    AgeRange? AgeRange,
    string? LocalAuthority,
    string? ProjectType,
    DateTime? ChangeDate
);
    public class FreeSchoolPipelineStatuses
    {
        public const string Pipeline = "Pipeline";
    }
}
