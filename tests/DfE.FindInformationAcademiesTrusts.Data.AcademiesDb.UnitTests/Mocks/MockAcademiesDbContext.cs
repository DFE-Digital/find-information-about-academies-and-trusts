using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    //application
    private readonly List<ApplicationEvent> _applicationEvents = [];

    private readonly List<ApplicationSetting> _applicationSettings = [];

    //cdm
    private readonly List<CdmSystemuser> _cdmSystemusers = [];

    private readonly List<CdmAccount> _cdmAccounts = [];

    //gias
    private readonly List<GiasEstablishment> _giasEstablishments = [];
    private readonly List<GiasGovernance> _giasGovernances = [];
    private readonly List<GiasGroup> _giasGroups = [];
    private readonly List<GiasGroupLink> _giasGroupLinks = [];

    //mis_mstr
    private readonly List<MisMstrEstablishmentFiat> _misMstrEstablishmentFiat = [];
    private readonly List<MisMstrFurtherEducationEstablishmentFiat> _misMstrFurtherEducationEstablishmentFiat = [];

    //mstr
    private readonly List<MstrTrust> _mstrTrusts = [];
    private readonly List<MstrAcademyConversions> _mstrAcademyConversions = [];
    private readonly List<MstrAcademyTransfers> _mstrAcademyTransfers = [];

    private readonly List<MstrFreeSchoolProject> _mstrFreeSchoolProjects = [];

    //tad
    private readonly List<TadTrustGovernance> _tadTrustGovernances = [];

    public MockAcademiesDbContext()
    {
        //application
        SetupMockDbContext(_applicationEvents, context => context.ApplicationEvents);
        SetupMockDbContext(_applicationSettings, context => context.ApplicationSettings);
        //cdm
        SetupMockDbContext(_cdmSystemusers, context => context.CdmSystemusers);
        SetupMockDbContext(_cdmAccounts, context => context.CdmAccounts);
        //gias
        SetupMockDbContext(_giasEstablishments, context => context.GiasEstablishments);
        SetupMockDbContext(_giasGovernances, context => context.GiasGovernances);
        SetupMockDbContext(_giasGroups, context => context.Groups);
        SetupMockDbContext(_giasGroupLinks, context => context.GiasGroupLinks);
        //mis_mstr
        SetupMockDbContext(_misMstrEstablishmentFiat, context => context.MisMstrEstablishmentsFiat);
        SetupMockDbContext(_misMstrFurtherEducationEstablishmentFiat,
            context => context.MisMstrFurtherEducationEstablishmentsFiat);
        //mstr
        SetupMockDbContext(_mstrAcademyConversions, context => context.MstrAcademyConversions);
        SetupMockDbContext(_mstrAcademyTransfers, context => context.MstrAcademyTransfers);
        SetupMockDbContext(_mstrFreeSchoolProjects, context => context.MstrFreeSchoolProjects);
        SetupMockDbContext(_mstrTrusts, context => context.MstrTrusts);
        //tad
        SetupMockDbContext(_tadTrustGovernances, context => context.TadTrustGovernances);

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

    public void SetupMockDbContext<T>(List<T> items, Expression<Func<IAcademiesDbContext, DbSet<T>>> dbContextTable)
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

    public void AddEstablishmentFiat(MisMstrEstablishmentFiat misMstrEstablishmentFiat)
    {
        _misMstrEstablishmentFiat.Add(misMstrEstablishmentFiat);
    }

    public void AddEstablishmentFiat(int urn, string? inspectionStartDate = null)
    {
        AddEstablishmentFiat(new MisMstrEstablishmentFiat
        {
            Urn = urn,
            InspectionStartDate = inspectionStartDate
        });
    }

    public void AddEstablishmentsFiat(params MisMstrEstablishmentFiat[] establishmentsFiat)
    {
        AddEstablishmentsFiat((IEnumerable<MisMstrEstablishmentFiat>)establishmentsFiat);
    }

    public void AddEstablishmentsFiat(IEnumerable<MisMstrEstablishmentFiat> establishmentsFiat)
    {
        _misMstrEstablishmentFiat.AddRange(establishmentsFiat);
    }

    public void AddFurtherEducationEstablishmentFiat(
        MisMstrFurtherEducationEstablishmentFiat misMstrFurtherEducationEstablishmentFiat)
    {
        _misMstrFurtherEducationEstablishmentFiat.Add(misMstrFurtherEducationEstablishmentFiat);
    }

    public void AddFurtherEducationEstablishmentsFiat(
        IEnumerable<MisMstrFurtherEducationEstablishmentFiat> furtherEducationEstablishmentsFiat)
    {
        _misMstrFurtherEducationEstablishmentFiat.AddRange(furtherEducationEstablishmentsFiat);
    }

    public void AddFurtherEducationEstablishmentsFiat(
        params MisMstrFurtherEducationEstablishmentFiat[] furtherEducationEstablishmentsFiat)
    {
        AddFurtherEducationEstablishmentsFiat(
            (IEnumerable<MisMstrFurtherEducationEstablishmentFiat>)furtherEducationEstablishmentsFiat);
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

    public void AddMstrFreeSchoolProject(
        string trustId,
        string stage,
        string routeOfProject,
        string? projectName = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        int? newUrn = null,
        string? localAuthority = null,
        DateTime? actualDateOpened = null)
    {
        _mstrFreeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = _mstrFreeSchoolProjects.GetNextId(e => e.SK),
            TrustID = trustId,
            Stage = stage,
            RouteOfProject = routeOfProject,
            ProjectName = projectName,
            StatutoryLowestAge = statutoryLowestAge,
            StatutoryHighestAge = statutoryHighestAge,
            NewURN = newUrn,
            LocalAuthority = localAuthority,
            ActualDateOpened = actualDateOpened
        });
    }

    public void AddMstrAcademyConversion(
        string trustId,
        string projectStatus,
        bool? inPrepare,
        bool? inComplete,
        string routeOfProject = "Conversion",
        string? projectName = null,
        int? urn = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        DateTime? expectedOpeningDate = null)
    {
        _mstrAcademyConversions.Add(new MstrAcademyConversions
        {
            SK = _mstrAcademyConversions.GetNextId(e => e.SK),
            TrustID = trustId,
            ProjectStatus = projectStatus,
            InPrepare = inPrepare,
            InComplete = inComplete,
            RouteOfProject = routeOfProject,
            ProjectName = projectName,
            URN = urn,
            StatutoryLowestAge = statutoryLowestAge,
            StatutoryHighestAge = statutoryHighestAge,
            ExpectedOpeningDate = expectedOpeningDate
        });
    }

    public void AddMstrAcademyTransfer(
        string newProvisionalTrustId,
        string academyTransferStatus,
        bool? inPrepare,
        bool? inComplete,
        string? academyName = null,
        int? academyUrn = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        string? localAuthority = null,
        DateTime? expectedTransferDate = null)
    {
        _mstrAcademyTransfers.Add(new MstrAcademyTransfers
        {
            SK = _mstrAcademyTransfers.GetNextId(e => e.SK),
            NewProvisionalTrustID = newProvisionalTrustId,
            AcademyTransferStatus = academyTransferStatus,
            InPrepare = inPrepare,
            InComplete = inComplete,
            AcademyName = academyName,
            AcademyURN = academyUrn,
            StatutoryLowestAge = statutoryLowestAge,
            StatutoryHighestAge = statutoryHighestAge,
            LocalAuthority = localAuthority,
            ExpectedTransferDate = expectedTransferDate
        });
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
