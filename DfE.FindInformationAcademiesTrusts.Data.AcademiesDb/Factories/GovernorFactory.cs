using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IGovernorFactory
{
    Governor CreateFrom(GiasGovernance giasGovernance, MstrTrustGovernance mstrGovernance);
}

public class GovernorFactory : IGovernorFactory
{
    public Governor CreateFrom(GiasGovernance giasGovernance, MstrTrustGovernance mstrGovernance)
    {
        return new Governor(
            giasGovernance.Gid!,
            giasGovernance.Uid!,
            GetFullName(giasGovernance),
            Enum.Parse<GovernanceRole>(giasGovernance.Role!),
            giasGovernance.AppointingBody!,
            giasGovernance.DateOfAppointment.ParseAsNullableDate(),
            giasGovernance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
            mstrGovernance.Email
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
