using Bogus;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class CdmAccountFaker
{
    private readonly Faker<CdmAccount> _cdmAccountFaker = new();
    private IEnumerable<Guid?>? _sfsoLeadIds;
    private IEnumerable<Guid?>? _trustRelationshipManagerIds;

    public CdmAccountFaker()
    {
        _cdmAccountFaker
            .RuleFor(a => a.SipTrustrelationshipmanager, f => f.PickRandom(_trustRelationshipManagerIds))
            .RuleFor(a => a.SipAmsdlead, f => f.PickRandom(_sfsoLeadIds));
    }

    public void SetSfsoLeads(IEnumerable<Guid?> systemUserIds)
    {
        _sfsoLeadIds = systemUserIds
            .Append(null); // Sfso Lead can sometimes be unassigned
    }

    public void SetTrustRelationshipManagers(IEnumerable<Guid?> systemUserIds)
    {
        _trustRelationshipManagerIds = systemUserIds
            .Append(null); // Trust relationship manager sometimes be unassigned
    }

    public CdmAccount Generate(string uid)
    {
        var result = _cdmAccountFaker.Generate();
        result.SipUid = uid;

        return result;
    }
}
