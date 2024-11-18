using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Extensions;

namespace DfE.FIAT.UnitTests.Extensions;

public class ContactRoleExtensionsTest
{
    [Fact]
    public void MapRoleToViewString_throws_when_mapping_is_invalid()
    {
        var sut = (ContactRole)9999;
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.MapRoleToViewString());
    }
}
