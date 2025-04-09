﻿using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class HistoricMembersModel(
    IDataSourceService dataSourceService,
    ILogger<HistoricMembersModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger)
{
    public const string SubPageName = "Historic members";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}
