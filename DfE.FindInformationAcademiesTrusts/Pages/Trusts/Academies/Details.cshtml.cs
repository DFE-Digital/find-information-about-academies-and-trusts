using DfE.FindInformationAcademiesTrusts.Data;
namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public AcademiesDetailsModel(ITrustProvider trustProvider) : base(trustProvider, "Academies in this trust")
    {
        PageTitle = "Academies details";
    }

    public string TabName => "Details";
}
