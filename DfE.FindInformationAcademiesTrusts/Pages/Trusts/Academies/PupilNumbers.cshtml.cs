using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class PupilNumbersModel : TrustsAreaModel, IAcademiesAreaModel
{
    public PupilNumbersModel(ITrustProvider trustProvider) : base(trustProvider, "Academies in this trust")
    {
        PageTitle = "Academies pupil numbers";
    }

    public string TabName => "Pupil numbers";
}
