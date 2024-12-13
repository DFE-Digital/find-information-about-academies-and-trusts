using System.Linq.Expressions;
using DfE.FIAT.Data.AcademiesDb.Contexts;
using DfE.FIAT.Data.AcademiesDb.Models.Cdm;
using DfE.FIAT.Data.AcademiesDb.Models.Gias;
using DfE.FIAT.Data.AcademiesDb.Models.Mis;
using DfE.FIAT.Data.AcademiesDb.Models.Mstr;
using DfE.FIAT.Data.AcademiesDb.Models.Ops;
using DfE.FIAT.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;

namespace DfE.FIAT.Data.AcademiesDb.UnitTests.Mocks;

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
    private readonly List<TadTrustGovernance> _tadTrustGovernances = [];
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
        SetupMockDbContext(_tadTrustGovernances, context => context.TadTrustGovernances);
        SetupMockDbContext(_applicationEvents, context => context.ApplicationEvents);
        SetupMockDbContext(_applicationSettings, context => context.ApplicationSettings);

        //Set up some unused data to ensure we are actually retrieving the right data in our tests
        var otherTrust = AddGiasGroup(groupName: "Some other trust");
        for (var i = 0; i < 15; i++)
        {
            //Completely unused
            AddGiasGroup(groupName: $"Unused {i}");
            AddMstrTrust(region: $"S{i}shire");
            AddGiasEstablishment(establishmentName: $"Unused {i}");

            //Entities linked to some other trust
            var otherAcademy = AddGiasEstablishment(establishmentName: $"Some other academy {i}");
            AddGiasGroupLink(otherAcademy, otherTrust);
            AddGiasGovernance(new GiasGovernance
                { Gid = $"{i}", Uid = otherTrust.GroupUid!, Forename1 = $"Governor {i}" });
            AddTadTrustGovernance(new TadTrustGovernance { Gid = $"{i}", Email = $"governor{i}@othertrust.com" });
        }
    }

    private void SetupMockDbContext<T>(List<T> items, Expression<Func<IAcademiesDbContext, DbSet<T>>> dbContextTable)
        where T : class
    {
        Setup(dbContextTable).Returns(new MockDbSet<T>(items).Object);
    }

    public MstrTrust AddMstrTrust(string? groupUid = null, string? region = "North East")
    {
        var mstrTrust = new MstrTrust
        {
            GroupUid = groupUid ?? (_mstrTrusts.Count + 1).ToString(),
            GORregion = region
        };
        _mstrTrusts.Add(mstrTrust);
        return mstrTrust;
    }

    public void AddGiasGroupLink(GiasEstablishment giasEstablishment, GiasGroup giasGroup)
    {
        AddGiasGroupLink(new GiasGroupLink
        {
            GroupUid = giasGroup.GroupUid,
            Urn = giasEstablishment.Urn.ToString(),
            GroupType = giasGroup.GroupType
        });
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
        foreach (var giasEstablishment in giasEstablishments)
        {
            AddGiasGroupLink(giasEstablishment, giasGroup);
        }
    }

    public void AddMisEstablishment(MisEstablishment misEstablishment)
    {
        _misEstablishments.Add(misEstablishment);
    }

    public void AddMisEstablishment(int? urn, string? inspectionStartDate = null)
    {
        AddMisEstablishment(new MisEstablishment
        {
            Urn = urn,
            InspectionStartDate = inspectionStartDate
        });
    }

    public void AddMisEstablishments(IEnumerable<MisEstablishment> misEstablishments)
    {
        _misEstablishments.AddRange(misEstablishments);
    }

    public void AddMisFurtherEducationEstablishment(MisFurtherEducationEstablishment misFurtherEducationEstablishment)
    {
        _misFurtherEducationEstablishments.Add(misFurtherEducationEstablishment);
    }

    public void AddMisFurtherEducationEstablishments(
        IEnumerable<MisFurtherEducationEstablishment> misFurtherEducationEstablishments)
    {
        _misFurtherEducationEstablishments.AddRange(misFurtherEducationEstablishments);
    }

    public void AddGiasEstablishment(GiasEstablishment giasEstablishment)
    {
        _giasEstablishments.Add(giasEstablishment);
    }

    public GiasEstablishment AddGiasEstablishment(int? urn = null, string? establishmentName = null)
    {
        var giasEstablishment = new GiasEstablishment
        {
            Urn = _giasEstablishments.GetNextId(e => e.Urn, urn),
            EstablishmentName = establishmentName ?? $"Academy {_giasEstablishments.Count + 1}"
        };
        AddGiasEstablishment(giasEstablishment);

        return giasEstablishment;
    }

    public void AddGiasEstablishments(IEnumerable<GiasEstablishment> giasEstablishments)
    {
        _giasEstablishments.AddRange(giasEstablishments);
    }

    public void AddApplicationEvent(string description,
        DateTime? dateTime,
        string? message = "Finished", char? eventType = 'I')
    {
        _applicationEvents.Add(new ApplicationEvent
        {
            Id = _applicationEvents.GetNextId(e => e.Id),
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

    public void AddGiasGroup(GiasGroup giasGroup)
    {
        _giasGroups.Add(giasGroup);
    }

    public GiasGroup AddGiasGroup(string? groupUid = null, string? groupName = null, string? groupId = null,
        string? groupType = null)
    {
        var nextGroupUid = _giasGroups.GetNextId(g => g.GroupUid!, groupUid);

        var giasGroup = new GiasGroup
        {
            GroupId = groupId ?? $"TR0{nextGroupUid}",
            GroupName = groupName ?? $"Trust {nextGroupUid}",
            GroupUid = nextGroupUid,
            GroupType = groupType ?? "Multi-academy trust"
        };
        AddGiasGroup(giasGroup);

        return giasGroup;
    }

    public void AddGiasGovernance(GiasGovernance governance)
    {
        _giasGovernances.Add(governance);
    }

    public void AddTadTrustGovernance(TadTrustGovernance tadTrustGovernance)
    {
        _tadTrustGovernances.Add(tadTrustGovernance);
    }
}

file static class MockDbSetExtensions
{
    public static int GetNextId<T>(this List<T> entities, Func<T, int> identifier, int? specifiedId = null)
    {
        var nextId = specifiedId ?? entities.Count + 1;

        //Don't allow duplicate IDs
        if (entities.Any(entity => identifier(entity) == nextId))
            entities.Remove(entities.Single(entity => identifier(entity) == nextId));

        return nextId;
    }

    public static string GetNextId<T>(this List<T> entities, Func<T, string> identifier, string? specifiedId = null)
    {
        var nextId = specifiedId ?? (entities.Count + 1).ToString();

        //Don't allow duplicate IDs
        if (entities.Any(entity => identifier(entity) == nextId))
            entities.Remove(entities.Single(entity => identifier(entity) == nextId));

        return nextId;
    }
}
