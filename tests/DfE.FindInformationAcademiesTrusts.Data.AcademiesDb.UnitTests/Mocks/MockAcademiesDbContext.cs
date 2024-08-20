using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    private readonly List<GiasGroupLink> _giasGroupLinks = [];
    private readonly List<MisEstablishment> _misEstablishments = [];
    private readonly List<MisFurtherEducationEstablishment> _misFurtherEducationEstablishments = [];
    private readonly List<CdmSystemuser> _cdmSystemusers = [];
    private readonly List<CdmAccount> _cdmAccounts = [];
    private readonly List<GiasGroup> _giasGroups = [];
    private readonly List<MstrTrust> _mstrTrusts = [];
    private readonly List<GiasEstablishment> _giasEstablishments = [];
    private readonly List<GiasGovernance> _giasGovernances = [];
    private readonly List<MstrTrustGovernance> _mstrTrustGovernances = [];
    private readonly List<ApplicationEvent> _applicationEvents = [];
    private readonly List<ApplicationSetting> _applicationSettings = [];

    public MockAcademiesDbContext()
    {
        SetupMockDbContext(_giasGroupLinks, context => context.GiasGroupLinks);
        SetupMockDbContext(_misEstablishments, context => context.MisEstablishments);
        SetupMockDbContext(_misFurtherEducationEstablishments, context => context.MisFurtherEducationEstablishments);
        SetupMockDbContext(_cdmSystemusers, context => context.CdmSystemusers);
        SetupMockDbContext(_cdmAccounts, context => context.CdmAccounts);
        SetupMockDbContext(_giasGroups, context => context.Groups);
        SetupMockDbContext(_mstrTrusts, context => context.MstrTrusts);
        SetupMockDbContext(_giasEstablishments, context => context.GiasEstablishments);
        SetupMockDbContext(_giasGovernances, context => context.GiasGovernances);
        SetupMockDbContext(_mstrTrustGovernances, context => context.MstrTrustGovernances);
        SetupMockDbContext(_applicationEvents, context => context.ApplicationEvents);
        SetupMockDbContext(_applicationSettings, context => context.ApplicationSettings);

        //Set up some unused data
        AddGiasGroups(15);
        AddGiasEstablishments(15);
        AddMstrTrusts(15);
        var otherTrust = AddGiasGroup("Some other trust");
        AddGovernancesLinkedToTrust(20, "Some other trust");
        LinkGiasEstablishmentsToGiasGroup(AddGiasEstablishments(5), otherTrust);
    }

    public MstrTrust AddMstrTrust(string groupUid, string? region = "North East")
    {
        var mstrTrust = new MstrTrust
        {
            GroupUid = groupUid, GORregion = region
        };
        _mstrTrusts.Add(mstrTrust);
        return mstrTrust;
    }

    public GiasGroup AddGiasGroup(string groupUid, string? groupName = "trust 1",
        string? groupType = "Multi-academy trust")
    {
        var giasGroup = new GiasGroup
            { GroupName = groupName, GroupUid = groupUid, GroupType = groupType, Ukprn = "my ukprn" };
        _giasGroups.Add(giasGroup);
        return giasGroup;
    }

    public void AddGiasGroup(GiasGroup giasGroup)
    {
        _giasGroups.Add(giasGroup);
    }

    public void AddGiasGroupLink(GiasGroupLink giasGroupLink)
    {
        _giasGroupLinks.Add(giasGroupLink);
    }

    public void AddGiasGroupLinks(IEnumerable<GiasGroupLink> giasGroupLink)
    {
        _giasGroupLinks.AddRange(giasGroupLink);
    }

    public void AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(IEnumerable<GiasEstablishment> giasEstablishments,
        GiasGroup giasGroup)
    {
        var newGroupLinks = giasEstablishments
            .Select(giasEstablishment => new GiasGroupLink
            {
                GroupUid = giasGroup.GroupUid,
                Urn = giasEstablishment.Urn.ToString(),
                GroupType = giasGroup.GroupType
            }).ToArray();
        _giasGroupLinks.AddRange(newGroupLinks);
    }

    public void AddMisEstablishments(IEnumerable<MisEstablishment> misEstablishments)
    {
        _misEstablishments.AddRange(misEstablishments);
    }
    
    public void AddMisFurtherEducationEstablishments(IEnumerable<MisFurtherEducationEstablishment> misFurtherEducationEstablishments)
    {
        _misFurtherEducationEstablishments.AddRange(misFurtherEducationEstablishments);
    }

    public GiasEstablishment AddGiasEstablishment(int urn, string? establishmentName = "my academy")
    {
        var giasEstablishment = new GiasEstablishment
        {
            Urn = urn,
            EstablishmentName = establishmentName
        };

        _giasEstablishments.Add(giasEstablishment);
        return giasEstablishment;
    }

    public void AddGiasEstablishments(IEnumerable<GiasEstablishment> giasEstablishments)
    {
        _giasEstablishments.AddRange(giasEstablishments);
    }

    public CdmSystemuser AddCdmSystemuser(string fullName, string email)
    {
        var cdmSystemuser = new CdmSystemuser
            { Systemuserid = Guid.NewGuid(), Fullname = fullName, Internalemailaddress = email };
        _cdmSystemusers.Add(cdmSystemuser);
        return cdmSystemuser;
    }

    public CdmAccount AddCdmAccount(string groupUid)
    {
        var cdmAccount = new CdmAccount { SipUid = groupUid };
        _cdmAccounts.Add(cdmAccount);
        return cdmAccount;
    }

    public void AddApplicationEvent(string description,
        DateTime? dateTime,
        string? message = "Finished", char? eventType = 'I')
    {
        var applicationEventId = _applicationEvents.Count;

        _applicationEvents.Add(new ApplicationEvent
        {
            Id = applicationEventId,
            DateTime = dateTime,
            Source = "source",
            UserName = "Test User",
            EventType = eventType,
            Level = 1,
            Code = 1,
            Severity = 'S',
            Description = description,
            Message = message,
            Trace = "test trace",
            ProcessID = 1,
            LineNumber = 2
        });
    }

    public void AddApplicationSetting(string key, DateTime? modified)
    {
        _applicationSettings.Add(new ApplicationSetting
        {
            Key = key,
            Modified = modified
        });
    }

    public List<GiasGroup> AddGiasGroups(int num)
    {
        return AddGiasGroupsForSearchTerm(string.Empty, num);
    }

    public List<GiasGroup> AddGiasGroupsForSearchTerm(string term, int num)
    {
        var numExisting = _giasGroups.Count;
        var newItems = new List<GiasGroup>();
        for (var i = 0; i < num; i++)
        {
            newItems.Add(new GiasGroup
            {
                GroupName = $"trust {term} {i}",
                GroupUid = $"{i + numExisting}",
                GroupId = $"TR0{i + numExisting}",
                GroupType = "Multi-academy trust"
            });
        }

        _giasGroups.AddRange(newItems);

        return newItems;
    }

    public List<MstrTrust> AddMstrTrusts(int num)
    {
        var numExisting = _mstrTrusts.Count;
        var newItems = new List<MstrTrust>();
        for (var i = 0; i < num; i++)
        {
            newItems.Add(new MstrTrust
            {
                GroupUid = $"{i + numExisting}",
                GORregion = "North East"
            });
        }

        _mstrTrusts.AddRange(newItems);

        return newItems;
    }

    public List<GiasEstablishment> AddGiasEstablishments(int num)
    {
        var numExisting = _giasEstablishments.Count;
        var newItems = new List<GiasEstablishment>();
        for (var i = 0; i < num; i++)
        {
            newItems.Add(new GiasEstablishment
            {
                Urn = i + numExisting,
                EstablishmentName = $"Academy {i + numExisting}"
            });
        }

        _giasEstablishments.AddRange(newItems);

        return newItems;
    }

    public void AddGiasGovernance(GiasGovernance governance)
    {
        var existing = _giasGovernances.FirstOrDefault(g => g.Gid == governance.Gid);
        if (existing is not null)
        {
            var index = _giasGovernances.IndexOf(existing);
            _giasGovernances[index] = governance;
            return;
        }

        _giasGovernances.Add(governance);
    }

    public List<(GiasGovernance, MstrTrustGovernance)> AddGovernancesLinkedToTrust(int num, string groupUid)
    {
        var numExistingGovernances = _giasGovernances.Count;
        var addedItems = new List<(GiasGovernance, MstrTrustGovernance)>();

        for (var i = 0; i < num; i++)
        {
            var gid = (i + numExistingGovernances).ToString();
            var giasGovernance = new GiasGovernance
            {
                Gid = gid,
                Uid = groupUid,
                Forename1 = $"Governor {i}"
            };
            var mstrTrustGovernance = new MstrTrustGovernance
            {
                Gid = gid,
                Forename1 = $"Governor {i}",
                Email = $"governor{i}@trust{groupUid}.com"
            };

            _giasGovernances.Add(giasGovernance);
            _mstrTrustGovernances.Add(mstrTrustGovernance);

            addedItems.Add((giasGovernance, mstrTrustGovernance));
        }

        return addedItems;
    }

    private void SetupMockDbContext<T>(List<T> items, Expression<Func<IAcademiesDbContext, DbSet<T>>> dbContextTable)
        where T : class
    {
        Setup(dbContextTable).Returns(new MockDbSet<T>(items).Object);
    }

    public List<(GiasEstablishment giasEstablishment, GiasGroupLink groupLink)> LinkGiasEstablishmentsToGiasGroup(
        IEnumerable<GiasEstablishment> giasEstablishments, GiasGroup giasGroup)
    {
        var establishmentGroupLinks = new List<(GiasEstablishment, GiasGroupLink)>();
        foreach (var giasEstablishment in giasEstablishments)
        {
            var giasGroupLink = new GiasGroupLink
            {
                GroupUid = giasGroup.GroupUid,
                Urn = giasEstablishment.Urn.ToString(),
                GroupType = giasGroup.GroupType
            };

            establishmentGroupLinks.Add((giasEstablishment, giasGroupLink));
            _giasGroupLinks.Add(giasGroupLink);
        }

        return establishmentGroupLinks;
    }

    public List<MisEstablishment> AddCurrentMisEstablishments(
        IEnumerable<int> urns)
    {
        var newItems = urns
            .Select(urn => new MisEstablishment
            {
                UrnAtTimeOfLatestFullInspection = urn
            }).ToList();

        _misEstablishments.AddRange(newItems);

        return newItems;
    }

    public List<MisEstablishment> AddPreviousMisEstablishments(IEnumerable<int> urns)
    {
        var newItems = urns
            .Select(urn =>
                new MisEstablishment { UrnAtTimeOfPreviousFullInspection = urn }).ToList();

        _misEstablishments.AddRange(newItems);

        return newItems;
    }

    public List<MisFurtherEducationEstablishment> CreateMisFurtherEducationEstablishments(IEnumerable<int> urns)
    {
        var newItems = urns
            .Select(urn => new MisFurtherEducationEstablishment { ProviderUrn = urn }).ToList();

        _misFurtherEducationEstablishments.AddRange(newItems);

        return newItems;
    }
}
