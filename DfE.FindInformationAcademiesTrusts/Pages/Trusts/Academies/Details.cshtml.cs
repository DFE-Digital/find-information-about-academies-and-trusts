using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel
{
    public AcademiesDetailsModel(ITrustProvider trustProvider) : base(trustProvider, "Academies in this trust")
    {
    }
}
