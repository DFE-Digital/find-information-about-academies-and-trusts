﻿using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrAcademyTransfer : IInComplete, IInPrepare
{
    public int SK { get; set; }
    public string? AcademyName { get; set; }
    public int? AcademyURN { get; set; }

    public required string AcademyTransferStatus { get; set; }

    public string? NewProvisionalTrustID { get; set; }

    public int? StatutoryLowestAge { get; set; }
    public int? StatutoryHighestAge { get; set; }
    public string? LocalAuthority { get; set; }
    public DateTime? ExpectedTransferDate { get; set; }
    public bool? InPrepare { get; set; }
    public bool? InComplete { get; set; }
    public DateTime? LastDataRefresh { get; set; }

}
