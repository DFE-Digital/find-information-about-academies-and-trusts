using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;

public class AcademiesInTrustDetailsModel(
    IDataSourceService dataSourceService,
    IOtherServicesLinkBuilder linkBuilder,
    ILogger<AcademiesInTrustDetailsModel> logger,
    ITrustService trustService,
    IAcademyService academyService,
    IExportService exportService,
    IDateTimeProvider dateTimeProvider,
    IFeatureManager featureManager
) : AcademiesInTrustAreaModel(
    dataSourceService,
    trustService,
    academyService,
    exportService,
    logger,
    dateTimeProvider,
    featureManager
)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = ViewConstants.AcademiesInTrustDetailsPageName };

    public AcademyDetailsServiceModel[] Academies { get; set; } = default!;
    public IOtherServicesLinkBuilder LinkBuilder { get; } = linkBuilder;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustDetailsAsync(Uid);

        return pageResult;
    }
}
