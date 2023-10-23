using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockAcademiesDbContext : Mock<IAcademiesDbContext>
{
    public List<Group> SetupMockDbContextGroups(int numMatches)
    {
        var groups = new List<Group>();
        for (var i = 0; i < numMatches; i++)
        {
            groups.Add(new Group
            {
                GroupName = $"trust {i}", GroupUid = $"{i}", GroupId = $"TR0{i}", GroupType = "Multi-academy trust"
            });
        }

        Setup(academiesDbContext => academiesDbContext.Groups)
            .Returns(new MockDbSet<Group>(groups).Object);

        return groups;
    }
}
