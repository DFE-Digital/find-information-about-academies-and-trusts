using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public IOtherServicesLinkBuilder LinkBuilder { get; }

    public AcademiesDetailsModel(ITrustProvider trustProvider, IOtherServicesLinkBuilder linkBuilder) :
        base(trustProvider, "Academies in this trust")
    {
        PageTitle = "Academies details";
        LinkBuilder = linkBuilder;
    }

    public string TabName => "Details";
}
