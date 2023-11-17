using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    private readonly List<GroupLink> _groupLinks = new();

    public MockAcademiesDbContext()
    {
        Setup(academiesDbContext => academiesDbContext.GroupLinks)
            .Returns(new MockDbSet<GroupLink>(_groupLinks).Object);
    }

    public List<Group> SetupMockDbContextGroups(int numMatches)
    {
        return SetupMockDbContext(numMatches,
            i => new Group
            {
                GroupName = $"trust {i}", GroupUid = $"{i}", GroupId = $"TR0{i}", GroupType = "Multi-academy trust"
            },
            academiesDbContext => academiesDbContext.Groups);
    }

    public List<MstrTrust> SetupMockDbContextMstrTrust(int numMatches)
    {
        return SetupMockDbContext(numMatches,
            i => new MstrTrust
            {
                GroupUid = $"{i}",
                GORregion = "North East"
            },
            academiesDbContext => academiesDbContext.MstrTrusts);
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

    public List<Governance> SetupMockDbContextGovernance(int numMatches, string groupUid)
    {
        return SetupMockDbContext(numMatches,
            i => new Governance
            {
                Uid = groupUid,
                Forename1 = $"Governor {i}"
            },
            academiesDbContext => academiesDbContext.Governances);
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

    public List<(GiasEstablishment giasEstablishment, GroupLink groupLink)> LinkGiasEstablishmentsToGroup(
        IEnumerable<GiasEstablishment> giasEstablishments, Group group)
    {
        var establishmentGroupLinks = new List<(GiasEstablishment, GroupLink)>();
        foreach (var giasEstablishment in giasEstablishments)
        {
            var groupLink = new GroupLink
            {
                GroupUid = group.GroupUid,
                Urn = giasEstablishment.Urn.ToString()
            };

            establishmentGroupLinks.Add((giasEstablishment, groupLink));
            _groupLinks.Add(groupLink);
        }

        return establishmentGroupLinks;
    }
}
