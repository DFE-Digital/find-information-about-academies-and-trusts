﻿using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrAcademyConversions
{
    public int SK { get; set; }

    public string? ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public int? URN { get; set; }
    public int? StatutoryLowestAge { get; set; }
    public int? StatutoryHighestAge { get; set; }
    public string? LocalAuthority { get; set; }
    public string? ProjectApplicationType { get; set; } = "Conversion";
    public required string ProjectStatus { get; set; }
    public required string RouteOfProject { get; set; }
    public string? EstablishmentName { get; set; }
    public DateTime? ExpectedOpeningDate { get; set; }
    public string? TrustID { get; set; }
    public required string DateSource { get; set; }
    public DateTime? LastDataRefresh { get; set; }
    public required string InPrepare { get; set; }
    public required string InComplete { get; set; }
}