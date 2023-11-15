using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class OfstedRatingsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public OfstedRatingsModel(ITrustProvider trustProvider) : base(trustProvider, "Academies in this trust")
    {
        PageTitle = "Academies Ofsted ratings";
    }

    public string TabName => "Ofsted ratings";
    
    public OfstedRatingCellModel GetOfstedRatingCellModel(DateTime academyJoinedDate, string rating,
        DateTime? ratingDate)
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = academyJoinedDate,
            Rating = rating,
            RatingDate = ratingDate
        };
    }
}
