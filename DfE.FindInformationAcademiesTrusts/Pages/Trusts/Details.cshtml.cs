using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel : TrustsAreaModel
{
    public ILinksToOtherServices LinksToOtherServices;

    public DetailsModel(ITrustProvider trustProvider, ILinksToOtherServices linksToOtherServices) : base(trustProvider,
        "Details")
    {
        LinksToOtherServices = linksToOtherServices;
    }

    public string SchoolsFinancialBenchmarkingLink()
    {
        if (Trust.IsMultiAcademyTrust())
        {
            return
                LinksToOtherServices.SchoolFinancialBenchmarkingServiceTrustLink(Trust.CompaniesHouseNumber);
        }

        return LinksToOtherServices.SchoolFinancialBenchmarkingServiceSchoolLink(GetSingleAcademyUrn());
    }

    public string FindSchoolPerformanceDataLink()
    {
        if (Trust.IsMultiAcademyTrust())
        {
            return LinksToOtherServices.FindSchoolPerformanceDataTrustLink(Trust.Uid);
        }

        return LinksToOtherServices.FindSchoolPerformanceDataSchoolLink(GetSingleAcademyUrn());
    }

    public bool IsMultiAcademyTrustOrHasAcademy()
    {
        return Trust.IsMultiAcademyTrust() || (Trust.IsSingleAcademyTrust() && Trust.Academies.Any());
    }

    private string GetSingleAcademyUrn()
    {
        return Trust.Academies.FirstOrDefault()?.Urn.ToString() ?? string.Empty;
    }
}
