using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;

namespace DfE.FIAT.Web.Services.Academy;

public record AcademyOfstedServiceModel(
    string Urn,
    string? EstablishmentName,
    DateTime DateAcademyJoinedTrust,
    OfstedRating PreviousOfstedRating,
    OfstedRating CurrentOfstedRating
)
{
    public BeforeOrAfterJoining WhenDidCurrentInspectionHappen
    {
        get
        {
            if (CurrentOfstedRating.InspectionDate is null)
            {
                return BeforeOrAfterJoining.NotYetInspected;
            }

            if (CurrentOfstedRating.InspectionDate >= DateAcademyJoinedTrust)
            {
                return BeforeOrAfterJoining.After;
            }

            // Must be CurrentOfstedRating.InspectionDate < DateAcademyJoinedTrust by process of elimination 

            return BeforeOrAfterJoining.Before;
        }
    }

    public BeforeOrAfterJoining WhenDidPreviousInspectionHappen
    {
        get
        {
            if (PreviousOfstedRating.InspectionDate is null)
            {
                return BeforeOrAfterJoining.NotYetInspected;
            }

            if (PreviousOfstedRating.InspectionDate >= DateAcademyJoinedTrust)
            {
                return BeforeOrAfterJoining.After;
            }

            // Must be PreviousOfstedRating.InspectionDate < DateAcademyJoinedTrust by process of elimination 

            return BeforeOrAfterJoining.Before;
        }
    }
}
