using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    private readonly List<GiasGroupLink> _giasGroupLinks = new();
    private List<GiasGroup>? _addedGiasGroups;
    private List<MstrTrust>? _addedMstrTrusts;

    public MockAcademiesDbContext()
    {
        Setup(academiesDbContext => academiesDbContext.GiasGroupLinks)
            .Returns(new MockDbSet<GiasGroupLink>(_giasGroupLinks).Object);
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
}
