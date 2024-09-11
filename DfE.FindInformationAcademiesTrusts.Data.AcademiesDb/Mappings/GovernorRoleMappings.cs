using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Mappings;

public static class GovernorRoleMappings
{
    public static Dictionary<string, GovernanceRole> GovernanceRoleStringToEnum =
        new()
        {
            { "Member", GovernanceRole.Member },
            { "Trustee", GovernanceRole.Trustee },
            { "Accounting Officer", GovernanceRole.AccountingOfficer },
            { "Chief Financial Officer", GovernanceRole.ChiefFinancialOfficer },
            { "Chair of Trustees", GovernanceRole.ChairOfTrustees }
        };

    public static Dictionary<GovernanceRole, string> GovernanceRoleEnumToString =
        new()
        {
            { GovernanceRole.Member, "Member" },
            { GovernanceRole.Trustee, "Trustee" },
            { GovernanceRole.AccountingOfficer, "Accounting Officer" },
            { GovernanceRole.ChiefFinancialOfficer, "Chief Financial Officer" },
            { GovernanceRole.ChairOfTrustees, "Chair of Trustees" }
        };
}
