using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IGovernorFactory
{
    Governor CreateFrom(GiasGovernance giasGovernance, TadTrustGovernance tadGovernance);
}

public class GovernorFactory : IGovernorFactory
{
    public Governor CreateFrom(GiasGovernance giasGovernance, TadTrustGovernance tadGovernance)
    {
        return new Governor(
            giasGovernance.Gid!,
            giasGovernance.Uid!,
            GetFullName(giasGovernance),
            giasGovernance.Role!,
            giasGovernance.AppointingBody!,
            giasGovernance.DateOfAppointment.ParseAsNullableDate(),
            giasGovernance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
            tadGovernance.Email
        );
    }

    private static string GetFullName(GiasGovernance giasGovernance)
    {
        var fullName = giasGovernance.Forename1!; //Forename1 is always populated

        if (!string.IsNullOrWhiteSpace(giasGovernance.Forename2))
            fullName += $" {giasGovernance.Forename2}";

        if (!string.IsNullOrWhiteSpace(giasGovernance.Surname))
            fullName += $" {giasGovernance.Surname}";

        return fullName;
    }
}
