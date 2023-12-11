using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    private readonly List<GiasGroupLink> _giasGroupLinks = new();
    private readonly List<MisEstablishment> _misEstablishments = new();
    private readonly List<MisFurtherEducationEstablishment> _misFurtherEducationEstablishments = new();
    private readonly List<CdmSystemuser> _cdmSystemusers = new();
    private readonly List<CdmAccount> _cdmAccounts = new();
    private List<GiasGroup>? _addedGiasGroups;
    private List<MstrTrust>? _addedMstrTrusts;

    public MockAcademiesDbContext()
    {
        Setup(academiesDbContext => academiesDbContext.GiasGroupLinks)
            .Returns(new MockDbSet<GiasGroupLink>(_giasGroupLinks).Object);
        Setup(academiesDbContext => academiesDbContext.MisEstablishments)
            .Returns(new MockDbSet<MisEstablishment>(_misEstablishments).Object);
        Setup(academiesDbContext => academiesDbContext.MisFurtherEducationEstablishments)
            .Returns(new MockDbSet<MisFurtherEducationEstablishment>(_misFurtherEducationEstablishments).Object);
        Setup(academiesDbContext => academiesDbContext.CdmSystemusers)
            .Returns(new MockDbSet<CdmSystemuser>(_cdmSystemusers).Object);
        Setup(academiesDbContext => academiesDbContext.CdmAccounts)
            .Returns(new MockDbSet<CdmAccount>(_cdmAccounts).Object);
    }

    public MstrTrust CreateMstrTrust(string groupUid)
    {
        var mstrTrust = new MstrTrust
        {
            GroupUid = groupUid, GORregion = "North East"
        };
        _addedMstrTrusts?.Add(mstrTrust);
        return mstrTrust;
    }

    public GiasGroup CreateGiasGroup(string groupUid)
    {
        var giasGroup = new GiasGroup
            { GroupName = "trust 1", GroupUid = groupUid, GroupType = "Multi-academy trust", Ukprn = "my ukprn" };
        _addedGiasGroups?.Add(giasGroup);
        return giasGroup;
    }

    public CdmSystemuser CreateCdmSystemuser(string fullName, string email)
    {
        var cdmSystemuser = new CdmSystemuser
            { Systemuserid = Guid.NewGuid(), Fullname = fullName, Internalemailaddress = email };
        _cdmSystemusers.Add(cdmSystemuser);
        return cdmSystemuser;
    }

    public CdmAccount CreateCdmAccount(string groupUid)
    {
        var cdmAccount = new CdmAccount { SipUid = groupUid };
        _cdmAccounts.Add(cdmAccount);
        return cdmAccount;
    }

    private static int _appEventId;

    private ApplicationEvent CreateApplicationEvent(DateTime? dateTime, string description, string message = "Finished",
        string source = "adf-t1ts-sips-dataflow", char eventType = 'I')
    {
        return new ApplicationEvent
        {
            Id = _appEventId++,
            DateTime = dateTime,
            Source = source,
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
        };
    }

    private ApplicationSetting createApplicationSetting(DateTime? modified, string key)
    {
        return new ApplicationSetting
        {
            Key = key,
            Modified = modified
        };
    }

    public List<GiasGroup> SetupMockDbContextGiasGroups(int numMatches)
    {
        _addedGiasGroups = SetupMockDbContext(numMatches,
            i => new GiasGroup
            {
                GroupName = $"trust {i}", GroupUid = $"{i}", GroupId = $"TR0{i}", GroupType = "Multi-academy trust"
            },
            academiesDbContext => academiesDbContext.Groups);
        return _addedGiasGroups;
    }

    public List<MstrTrust> SetupMockDbContextMstrTrust(int numMatches)
    {
        _addedMstrTrusts = SetupMockDbContext(numMatches,
            i => new MstrTrust
            {
                GroupUid = $"{i}",
                GORregion = "North East"
            },
            academiesDbContext => academiesDbContext.MstrTrusts);
        return _addedMstrTrusts;
    }

    public List<GiasEstablishment> SetupMockDbContextGiasEstablishment(int numMatches)
    {
        return SetupMockDbContext(numMatches,
            i => new GiasEstablishment
            {
                Urn = i,
                EstablishmentName = $"Academy {i}"
            },
            academiesDbContext => academiesDbContext.GiasEstablishments);
    }

    public List<GiasGovernance> SetupMockDbContextGiasGovernance(int numMatches, string groupUid)
    {
        return SetupMockDbContext(numMatches,
            i => new GiasGovernance
            {
                Uid = groupUid,
                Gid = i.ToString(),
                Forename1 = $"Governor {i}"
            },
            academiesDbContext => academiesDbContext.GiasGovernances);
    }

    public List<MstrTrustGovernance> SetupMockDbContextMstrTrustGovernance(int numMatches)
    {
        return SetupMockDbContext(numMatches,
            i => new MstrTrustGovernance
            {
                Gid = i.ToString(),
                Forename1 = $"Governor {i}"
            },
            academiesDbContext => academiesDbContext.MstrTrustGovernances);
    }

    public List<ApplicationEvent> SetupMockDbContextOpsApplicationEvents(DateTime time)
    {
        var items = new List<ApplicationEvent>
        {
            CreateApplicationEvent(time.AddDays(-1), "GIAS_Daily"),
            CreateApplicationEvent(time.AddDays(-2), "GIAS_Daily"),
            CreateApplicationEvent(time.AddDays(-1), "MSTR_Daily"),
            CreateApplicationEvent(time.AddDays(-2), "MSTR_Daily"),
            CreateApplicationEvent(time.AddDays(-1), "CDM_Daily"),
            CreateApplicationEvent(time.AddDays(-2), "CDM_Daily")
        };

        Setup(academiesTable => academiesTable.ApplicationEvents)
            .Returns(new MockDbSet<ApplicationEvent>(items).Object);
        return items;
    }

    public List<ApplicationEvent> SetupEmptyMockDbContextOpsApplicationEvents()
    {
        var items = new List<ApplicationEvent>();

        Setup(academiesTable => academiesTable.ApplicationEvents)
            .Returns(new MockDbSet<ApplicationEvent>(items).Object);
        return items;
    }

    public List<ApplicationEvent> SetupInvalidMockDbContextOpsApplicationEvents(DateTime time)
    {
        var items = new List<ApplicationEvent>
        {
            CreateApplicationEvent(time.AddDays(-1), "Wrong Description"),
            CreateApplicationEvent(time.AddDays(-1), "GIAS_Daily", "Started"),
            CreateApplicationEvent(time.AddDays(-2), "GIAS_Daily", source: "test-Source"),
            CreateApplicationEvent(time.AddDays(-3), "GIAS_Daily", eventType: 'E'),
            CreateApplicationEvent(time.AddDays(-1), "MSTR_Daily", "Started"),
            CreateApplicationEvent(time.AddDays(-2), "MSTR_Daily", source: "test-Source"),
            CreateApplicationEvent(time.AddDays(-3), "MSTR_Daily", eventType: 'E'),
            CreateApplicationEvent(time.AddDays(-1), "CDM_Daily", "Started"),
            CreateApplicationEvent(time.AddDays(-2), "CDM_Daily", source: "test-Source"),
            CreateApplicationEvent(time.AddDays(-3), "CDM_Daily", eventType: 'E')
        };

        Setup(academiesTable => academiesTable.ApplicationEvents)
            .Returns(new MockDbSet<ApplicationEvent>(items).Object);
        return items;
    }

    public List<ApplicationSetting> SetupMockDbContextOpsApplicationSettings(DateTime time)
    {
        var items = new List<ApplicationSetting>
        {
            createApplicationSetting(time.AddDays(-1), "ManagementInformationSchoolTableData CSV Filename"),
            createApplicationSetting(time.AddDays(-2),
                "ManagementInformationFurtherEducationSchoolTableData CSV Filename")
        };

        Setup(academiesTable => academiesTable.ApplicationSettings)
            .Returns(new MockDbSet<ApplicationSetting>(items).Object);
        return items;
    }

    public List<ApplicationSetting> SetupEmptyMockDbContextOpsApplicationSettings()
    {
        var items = new List<ApplicationSetting>();

        Setup(academiesTable => academiesTable.ApplicationSettings)
            .Returns(new MockDbSet<ApplicationSetting>(items).Object);
        return items;
    }

    public List<ApplicationSetting> SetupInvalidMockDbContextOpsApplicationSettings(DateTime time)
    {
        var items = new List<ApplicationSetting>
        {
            createApplicationSetting(time.AddDays(-1), "Other Filename"),
            createApplicationSetting(time.AddDays(-2), "test")
        };

        Setup(academiesTable => academiesTable.ApplicationSettings)
            .Returns(new MockDbSet<ApplicationSetting>(items).Object);
        return items;
    }

    private List<T> SetupMockDbContext<T>(int numMatches, Func<int, T> itemCreator,
        Expression<Func<IAcademiesDbContext, DbSet<T>>> dbContextTable) where T : class
    {
        var items = new List<T>();
        for (var i = 0; i < numMatches; i++)
        {
            items.Add(itemCreator(i));
        }

        Setup(dbContextTable)
            .Returns(new MockDbSet<T>(items).Object);

        return items;
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
                Urn = giasEstablishment.Urn.ToString()
            };

            establishmentGroupLinks.Add((giasEstablishment, giasGroupLink));
            _giasGroupLinks.Add(giasGroupLink);
        }

        return establishmentGroupLinks;
    }

    public List<MisEstablishment> CreateCurrentMisEstablishments(
        IEnumerable<GiasEstablishment> giasEstablishmentsLinkedToTrust)
    {
        var misEstablishments = new List<MisEstablishment>();
        foreach (var giasEstablishment in giasEstablishmentsLinkedToTrust)
        {
            var misEstablishment = new MisEstablishment
            {
                UrnAtTimeOfLatestFullInspection = giasEstablishment.Urn
            };
            _misEstablishments.Add(misEstablishment);
            misEstablishments.Add(misEstablishment);
        }

        return misEstablishments;
    }

    public List<MisEstablishment> CreatePreviousMisEstablishments(
        IEnumerable<GiasEstablishment> giasEstablishmentsLinkedToTrust)
    {
        var misEstablishments = new List<MisEstablishment>();
        foreach (var giasEstablishment in giasEstablishmentsLinkedToTrust)
        {
            var misEstablishment = new MisEstablishment
            {
                UrnAtTimeOfPreviousFullInspection = giasEstablishment.Urn
            };
            _misEstablishments.Add(misEstablishment);
            misEstablishments.Add(misEstablishment);
        }

        return misEstablishments;
    }

    public List<MisFurtherEducationEstablishment> CreateMisFurtherEducationEstablishments(
        GiasEstablishment[] giasEstablishmentsLinkedToTrust)
    {
        var misFurtherEducationEstablishments = new List<MisFurtherEducationEstablishment>();
        foreach (var giasEstablishment in giasEstablishmentsLinkedToTrust)
        {
            var misFurtherEducationEstablishment = new MisFurtherEducationEstablishment
            {
                ProviderUrn = giasEstablishment.Urn
            };
            _misFurtherEducationEstablishments.Add(misFurtherEducationEstablishment);
            misFurtherEducationEstablishments.Add(misFurtherEducationEstablishment);
        }

        return misFurtherEducationEstablishments;
    }
}
