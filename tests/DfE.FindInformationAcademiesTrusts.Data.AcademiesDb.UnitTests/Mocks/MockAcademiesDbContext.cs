using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext
{
    public readonly IAcademiesDbContext Object = Substitute.For<IAcademiesDbContext>();

    //application
    private readonly MockDbSet<ApplicationEvent> _applicationEvents = new();
    private readonly MockDbSet<ApplicationSetting> _applicationSettings = new();

    //gias
    private readonly MockDbSet<GiasEstablishment> _giasEstablishments = new();
    private readonly MockDbSet<GiasGovernance> _giasGovernances = new();
    private readonly MockDbSet<GiasGroup> _giasGroups = new();
    private readonly MockDbSet<GiasGroupLink> _giasGroupLinks = new();
    private readonly MockDbSet<GiasEstablishmentLink> _giasEstablishmentLinks = new();

    //mis_mstr
    private readonly MockDbSet<MisMstrEstablishmentFiat> _misMstrEstablishmentFiat = new();

    private readonly MockDbSet<MisMstrFurtherEducationEstablishmentFiat> _misMstrFurtherEducationEstablishmentFiat =
        new();

    //mstr
    private readonly MockDbSet<MstrTrust> _mstrTrusts = new();
    private readonly MockDbSet<MstrAcademyConversion> _mstrAcademyConversions = new();
    private readonly MockDbSet<MstrAcademyTransfer> _mstrAcademyTransfers = new();
    private readonly MockDbSet<MstrFreeSchoolProject> _mstrFreeSchoolProjects = new();

    //sharepoint
    private readonly MockDbSet<SharepointTrustDocLink> _sharepointTrustDocLinks = new();

    //tad
    private readonly MockDbSet<TadTrustGovernance> _tadTrustGovernances = new();

    public MockAcademiesDbContext()
    {
        //application
        Object.ApplicationEvents.Returns(_applicationEvents.Object);
        Object.ApplicationSettings.Returns(_applicationSettings.Object);
        //gias
        Object.GiasEstablishments.Returns(_giasEstablishments.Object);
        Object.GiasGovernances.Returns(_giasGovernances.Object);
        Object.Groups.Returns(_giasGroups.Object);
        Object.GiasGroupLinks.Returns(_giasGroupLinks.Object);
        Object.GiasEstablishmentLink.Returns(_giasEstablishmentLinks.Object);
        //mis_mstr
        Object.MisMstrEstablishmentsFiat.Returns(_misMstrEstablishmentFiat.Object);
        Object.MisMstrFurtherEducationEstablishmentsFiat.Returns(_misMstrFurtherEducationEstablishmentFiat.Object);
        //mstr
        Object.MstrAcademyConversions.Returns(_mstrAcademyConversions.Object);
        Object.MstrAcademyTransfers.Returns(_mstrAcademyTransfers.Object);
        Object.MstrFreeSchoolProjects.Returns(_mstrFreeSchoolProjects.Object);
        Object.MstrTrusts.Returns(_mstrTrusts.Object);
        //sharepoint
        Object.SharepointTrustDocLinks.Returns(_sharepointTrustDocLinks.Object);
        //tad
        Object.TadTrustGovernances.Returns(_tadTrustGovernances.Object);

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
            AddMstrFreeSchoolProject(otherTrust.GroupId!);
        }
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

    public void AddGiasEstablishmentLink(GiasEstablishmentLink giasEstablishmentLink)
    {
        _giasEstablishmentLinks.Add(giasEstablishmentLink);
    }

    public void AddGiasEstablishmentLinks(IEnumerable<GiasEstablishmentLink> giasEstablishmentLinks)
    {
        _giasEstablishmentLinks.AddRange(giasEstablishmentLinks);
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

    public void AddMstrFreeSchoolProject(string trustId,
        string projectApplicationType = "Presumption",
        string stage = PipelineStatuses.FreeSchoolPipeline,
        string? projectName = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        int? newUrn = null,
        string? localAuthority = null,
        DateTime? provisionalOpeningDate = null,
        DateTime? lastDataRefresh = null)
    {
        _mstrFreeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = _mstrFreeSchoolProjects.GetNextId(e => e.SK),
            TrustID = trustId,
            Stage = stage,
            RouteOfProject = "Free School",
            ProjectApplicationType = projectApplicationType,
            ProjectName = projectName,
            StatutoryLowestAge = statutoryLowestAge,
            StatutoryHighestAge = statutoryHighestAge,
            NewURN = newUrn,
            LocalAuthority = localAuthority,
            ProvisionalOpeningDate = provisionalOpeningDate,
            LastDataRefresh = lastDataRefresh
        });
    }

    public void AddMstrAcademyConversion(string trustId, AdvisoryType advisoryType, string projectStatus,
        string projectName, string? daoProgress = null)
    {
        AddMstrAcademyConversion(trustId, projectStatus,
            advisoryType == AdvisoryType.PreAdvisory,
            advisoryType == AdvisoryType.PostAdvisory,
            projectName: projectName, daoProgress: daoProgress);
    }

    public void AddMstrAcademyConversion(
        string trustId,
        string projectStatus,
        bool? inPrepare,
        bool? inComplete,
        string routeOfProject = "Converter",
        string? projectName = null,
        int? urn = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        DateTime? expectedOpeningDate = null,
        string? daoProgress = null)
    {
        _mstrAcademyConversions.Add(new MstrAcademyConversion
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
            ExpectedOpeningDate = expectedOpeningDate,
            DaoProgress = daoProgress
        });
    }

    public void AddMstrAcademyTransfer(string newProvisionalTrustId,
        string academyTransferStatus,
        bool? inPrepare,
        bool? inComplete,
        string? academyName = null,
        int? academyUrn = null,
        int? statutoryLowestAge = null,
        int? statutoryHighestAge = null,
        string? localAuthority = null,
        DateTime? expectedTransferDate = null,
        DateTime? lastDataRefresh = null)
    {
        _mstrAcademyTransfers.Add(new MstrAcademyTransfer
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
            ExpectedTransferDate = expectedTransferDate,
            LastDataRefresh = lastDataRefresh
        });
    }

    public void AddTrustDocLink(SharepointTrustDocLink sharepointTrustDocLink)
    {
        _sharepointTrustDocLinks.Add(sharepointTrustDocLink);
    }

    public SharepointTrustDocLink[] AddTrustDocLinks(string trustReferenceNumber, string folderPrefix, int number)
    {
        var sharepointTrustDocLinks = Enumerable.Range(1, number).Select(i =>
            new SharepointTrustDocLink
            {
                FolderPrefix = folderPrefix,
                TrustRefNumber = trustReferenceNumber,
                DocumentFilename = $"Trust Document {i}",
                DocumentLink = $"www.trustDocumentLink{i}.com",
                FolderYear = 2000 + i
            }).ToArray();

        _sharepointTrustDocLinks.AddRange(sharepointTrustDocLinks);
        return sharepointTrustDocLinks;
    }
}
