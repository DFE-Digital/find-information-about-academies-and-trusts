using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class DetailsModel : TrustsAreaModel
{
    public DetailsModel(ITrustProvider trustProvider) : base(trustProvider, "Details")
    {
    }
}
