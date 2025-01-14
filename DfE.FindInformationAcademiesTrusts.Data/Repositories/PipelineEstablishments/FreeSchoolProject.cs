namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments
{
    public class FreeSchoolProject
    {
        public int SK { get; set; }

        public string? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectApplicationType { get; set; }
        public string? LocalAuthority { get; set; }
        public string? Region { get; set; }
        public string? SchoolPhase { get; set; }
        public string? SchoolType { get; set; }
        public string? ProjectStatus { get; set; }
        public string? Stage { get; set; }
        public string? RouteOfProject { get; set; }
        public int? StatutoryLowestAge { get; set; }
        public int? StatutoryHighestAge { get; set; }
        public int? NewURN { get; set; }
        public string? EstablishmentName { get; set; }
        public DateTime? ActualDateOpened { get; set; }
        public string? TrustID { get; set; }
        public string? TrustName { get; set; }
        public string? TrustType { get; set; }
        public string? CompaniesHouseNumber { get; set; }
        public string? DateSource { get; set; }
        public DateTime? LastDataRefresh { get; set; }
    }
    public class FreeSchoolProjectStatuses
    {
        public const string Pipeline = "Free School Pipeline";
    }
    public class AcademyConversions
    {
        public int SK { get; set; }

        public string? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectApplicationType { get; set; }
        public string? LocalAuthority { get; set; }
        public string? Region { get; set; }
        public string? SchoolPhase { get; set; }
        public string? SchoolType { get; set; }
        public string? ProjectStatus { get; set; }
        public string? Stage { get; set; }
        public string? RouteOfProject { get; set; }
        public int? StatutoryLowestAge { get; set; }
        public int? StatutoryHighestAge { get; set; }
        public int? URN { get; set; }
        public string? EstablishmentName { get; set; }
        public DateTime? ActualDateOpened { get; set; }
        public string? TrustID { get; set; }
        public string? TrustName { get; set; }
        public string? TrustType { get; set; }
        public string? CompaniesHouseNumber { get; set; }
        public string? DateSource { get; set; }
        public DateTime? LastDataRefresh { get; set; }
    }
}