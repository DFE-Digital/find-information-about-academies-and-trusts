using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public record SingleHeadlineGradeCellsModel(
    OfstedRating SingleHeadlineGradeRating,
    BeforeOrAfterJoining BeforeOrAfterJoining,
    bool IsCurrent)
{
    public BeforeOrAfterTagModel BeforeOrAfterTagModel { get; } = new(BeforeOrAfterJoining, IsCurrent);
}
