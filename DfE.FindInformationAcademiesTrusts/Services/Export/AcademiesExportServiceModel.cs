namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public record AcademiesExportServiceModel
    {
        public string EstablishmentName { get; init; } = string.Empty;
        public string Urn { get; init; } = string.Empty;
        public string LocalAuthority { get; init; } = string.Empty;
        public string TypeOfEstablishment { get; init; } = string.Empty;
        public string UrbanRural { get; init; } = string.Empty;
        public DateTime DateAcademyJoinedTrust { get; init; }
        public string PreviousOfstedRating { get; init; } = string.Empty;
        public string IsPreviousOfstedRatingBeforeOrAfterJoining { get; init; } = string.Empty;
        public string PreviousOfstedInspectionDate { get; init; } = string.Empty;
        public string CurrentOfstedRating { get; init; } = string.Empty;
        public string IsCurrentOfstedRatingBeforeOrAfterJoining { get; init; } = string.Empty;
        public string CurrentOfstedInspectionDate { get; init; } = string.Empty;
        public string PhaseOfEducation { get; init; } = string.Empty;
        public string AgeRange { get; init; } = string.Empty;
        public string NumberOfPupils { get; init; } = string.Empty;
        public string SchoolCapacity { get; init; } = string.Empty;
        public string PercentageFull { get; init; } = string.Empty;
        public string PercentageFreeSchoolMeals { get; init; } = string.Empty;
    }

}
