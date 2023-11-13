using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class OverviewModel : TrustsAreaModel
{
    public OverviewModel(ITrustProvider trustProvider) : base(trustProvider, "Overview")
    {
    }
}
