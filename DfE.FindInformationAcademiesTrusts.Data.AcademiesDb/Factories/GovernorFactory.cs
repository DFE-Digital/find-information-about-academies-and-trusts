using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IGovernorFactory
{
    Governor CreateFrom(GiasGovernance giasGovernance);
}

public class GovernorFactory : IGovernorFactory
{
    public Governor CreateFrom(GiasGovernance giasGovernance)
    {
        return new Governor(
            giasGovernance.Gid!,
            giasGovernance.Uid!,
            GetFullName(giasGovernance),
            giasGovernance.Role,
            giasGovernance.AppointingBody,
            giasGovernance.DateOfAppointment.ParseAsNullableDate(),
            giasGovernance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
            null
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
