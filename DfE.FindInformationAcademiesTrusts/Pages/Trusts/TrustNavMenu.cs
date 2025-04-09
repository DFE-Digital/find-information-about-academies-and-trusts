using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.NavMenu;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public static class TrustNavMenu
{
    public static NavLink[] GetServiceNavLinks(ITrustsAreaModel activePage)
    {
        return
        [
            GetServiceNavLinkTo<OverviewAreaModel>(OverviewAreaModel.PageName, "/Trusts/Overview/TrustDetails",
                activePage),
            GetServiceNavLinkTo<ContactsAreaModel>(ContactsAreaModel.PageName, "/Trusts/Contacts/InDfe", activePage),
            GetServiceNavLinkTo<AcademiesAreaModel>(
                $"{AcademiesAreaModel.PageName} ({activePage.TrustSummary.NumberOfAcademies})",
                "/Trusts/Academies/InTrust/Details", activePage),
            GetServiceNavLinkTo<OfstedAreaModel>(OfstedAreaModel.PageName, "/Trusts/Ofsted/SingleHeadlineGrades",
                activePage),
            GetServiceNavLinkTo<FinancialDocumentsAreaModel>(FinancialDocumentsAreaModel.PageName,
                "/Trusts/FinancialDocuments/FinancialStatements", activePage),
            GetServiceNavLinkTo<GovernanceAreaModel>(GovernanceAreaModel.PageName, "/Trusts/Governance/TrustLeadership",
                activePage)
        ];
    }

    private static NavLink GetServiceNavLinkTo<T>(string linkDisplayText, string aspPage, ITrustsAreaModel activePage)
    {
        return new NavLink(activePage is T,
            "Trust",
            linkDisplayText,
            aspPage,
            $"{linkDisplayText}-nav".Kebabify(),
            new Dictionary<string, string> { { "uid", activePage.Uid } });
    }
}
