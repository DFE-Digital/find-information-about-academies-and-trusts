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

    public List<MstrTrust> SetupMockDbContextMstrTrust(int numMatches)
    {
        var mstrTrusts = new List<MstrTrust>();
        for (var i = 0; i < numMatches; i++)
        {
            mstrTrusts.Add(new MstrTrust
            {
                GroupUid = $"{i}",
                GORregion = "North East"
            });
        }

        Setup(academiesDbContext => academiesDbContext.MstrTrusts)
            .Returns(new MockDbSet<MstrTrust>(mstrTrusts).Object);

        return mstrTrusts;
    }
}
