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

        if (Trust.IsSingleAcademyTrust())
        {
            return LinksToOtherServices.SchoolFinancialBenchmarkingServiceSchoolLink(GetSingleAcademyUrn());
        }

        return string.Empty;
    }

    public string FindSchoolPerformanceDataLink()
    {
        if (Trust.IsMultiAcademyTrust())
        {
            return LinksToOtherServices.FindSchoolPerformanceDataTrustLink(Trust.Uid);
        }

        if (Trust.IsSingleAcademyTrust())
        {
            return LinksToOtherServices.FindSchoolPerformanceDataSchoolLink(GetSingleAcademyUrn());
        }

        return string.Empty;
    }

    private string GetSingleAcademyUrn()
    {
        return Trust.Academies.FirstOrDefault()?.Urn.ToString() ?? string.Empty;
    }
}
