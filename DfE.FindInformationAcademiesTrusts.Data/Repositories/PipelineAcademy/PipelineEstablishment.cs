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
    public static class PipelineStatuses
    {
        // Free schools
        public const string FreeSchoolPipeline = "Pipeline";

        // Conversions
        public const string ApprovedForAO = "Approved for AO";
        public const string ConverterPreAOC = "Converter Pre-AO (C)";
        public const string ConverterPreAO = "Converter Pre-AO";
        public const string Deferred = "Deferred (C)";
        public const string DirectiveAcademyOrders = "Directive Academy Orders";
        public const string AwaitingModeration = "Awaiting Moderation";

        // Transfers
        public const string ConsideringAcademyTransfer = "Considering academy transfer";
        public const string InProcessOfAcademyTransfer = "In process of academy transfer";

    }

    public enum AdvisoryType
    {
        PreAdvisory,
        PostAdvisory
    }

}