using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Extensions;

public class IQueryableExtensionsTests
{
    [Fact]
    public void GiasGroup_Trusts_should_filter_out_null_groupId_as_it_is_never_null_for_a_trust()
    {
        var trustWithGroupId = new GiasGroup
        {
            GroupUid = "1234",
            GroupId = "TR001234",
            GroupName = "My Trust",
            GroupType = "Multi-academy trust",
            GroupStatusCode = "OPEN"
        };

        GiasGroup[] data =
        [
            trustWithGroupId,
            new()
            {
                GroupUid = "5678",
                GroupId = null,
                GroupName = "Not a valid trust",
                GroupType = "Multi-academy trust",
                GroupStatusCode = "OPEN"
            }
        ];

        data.AsQueryable().Trusts().Should()
            .ContainSingle()
            .Which.Should().Be(trustWithGroupId);
    }

    [Fact]
    public void GiasGroup_Trusts_should_filter_on_group_type()
    {
        var validMultiAcademyTrust = new GiasGroup
        {
            GroupUid = "1234",
            GroupId = "TR001234",
            GroupName = "My Trust",
            GroupType = "Multi-academy trust",
            GroupStatusCode = "OPEN"
        };
        var validSingleAcademyTrust = new GiasGroup
        {
            GroupUid = "1234",
            GroupId = "TR001234",
            GroupName = "My Trust",
            GroupType = "Single-academy trust",
            GroupStatusCode = "OPEN"
        };

        GiasGroup[] data =
        [
            validMultiAcademyTrust,
            validSingleAcademyTrust,
            new()
            {
                GroupUid = "5678",
                GroupId = "Some ID",
                GroupName = "Not a trust",
                GroupType = "Federation",
                GroupStatusCode = "OPEN"
            }
        ];

        data.AsQueryable().Trusts().Should()
            .BeEquivalentTo([validMultiAcademyTrust, validSingleAcademyTrust]);
    }
    
    [Fact]
    public void GiasGroupLink_SingleAcademyTrusts_should_filter_out_null_groupId_as_it_is_never_null_for_a_trust()
    {
        var trustWithGroupId = new GiasGroupLink
        {
            Urn = "123456",
            GroupUid = "1234",
            GroupId = "TR001234",
            GroupName = "My Trust",
            GroupType = "Single-academy trust",
            GroupStatusCode = "OPEN"
        };

        GiasGroupLink[] data =
        [
            trustWithGroupId,
            new()
            {
                Urn = "789123",
                GroupUid = "5678",
                GroupId = null,
                GroupName = "Not a valid trust",
                GroupType = "Single-academy trust",
                GroupStatusCode = "OPEN"
            }
        ];

        data.AsQueryable().SingleAcademyTrusts().Should()
            .ContainSingle()
            .Which.Should().Be(trustWithGroupId);
    }

    [Fact]
    public void GiasGroupLink_SingleAcademyTrusts_should_filter_on_group_type()
    {
        var validSingleAcademyTrust = new GiasGroupLink
        {
            Urn = "123456",
            GroupUid = "1234",
            GroupId = "TR001234",
            GroupName = "My Trust",
            GroupType = "Single-academy trust",
            GroupStatusCode = "OPEN"
        };

        GiasGroupLink[] data =
        [
            validSingleAcademyTrust,
            new()
            {
                Urn = "234567",
                GroupUid = "1234",
                GroupId = "TR001234",
                GroupName = "My Trust",
                GroupType = "Multi-academy trust",
                GroupStatusCode = "OPEN"
            },
            new()
            {
                Urn = "789123",
                GroupUid = "5678",
                GroupId = "Some ID",
                GroupName = "Not a trust",
                GroupType = "Federation",
                GroupStatusCode = "OPEN"
            }
        ];

        data.AsQueryable().SingleAcademyTrusts().Should()
            .ContainSingle()
            .Which.Should().Be(validSingleAcademyTrust);
    }
}
