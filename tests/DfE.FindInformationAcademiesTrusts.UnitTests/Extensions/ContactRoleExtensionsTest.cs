using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class ContactRoleExtensionsTest
{
    [Fact]
    public void MapRoleToViewString_throws_when_mapping_is_invalid()
    {
        var sut = (ContactRole)9999;
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.MapRoleToViewString());
    }
}
