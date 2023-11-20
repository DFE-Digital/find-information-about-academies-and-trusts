using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IGovernorFactory
{
    Governor CreateFrom(Governance g);
}

public class GovernorFactory : IGovernorFactory
{
    public Governor CreateFrom(Governance governance)
    {
        return new Governor(
            governance.Gid!,
            governance.Uid!,
            GetFullName(governance),
            governance.Role,
            governance.AppointingBody,
            governance.DateOfAppointment.ParseAsNullableDate(),
            governance.DateTermOfOfficeEndsEnded.ParseAsNullableDate(),
            null
        );
    }

    private static string GetFullName(Governance governance)
    {
        var fullName = governance.Forename1!; //Forename1 is always populated

        if (!string.IsNullOrWhiteSpace(governance.Forename2))
            fullName += $" {governance.Forename2}";

        if (!string.IsNullOrWhiteSpace(governance.Surname))
            fullName += $" {governance.Surname}";

        return fullName;
    }
}
