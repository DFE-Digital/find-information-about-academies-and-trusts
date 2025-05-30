using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class AcademyRepositoryTests
{
    private const string GroupUid = "1234";
    private readonly AcademyRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();

    public AcademyRepositoryTests()
    {
        _sut = new AcademyRepository(_mockAcademiesDbContext.Object);
    }

    [Fact]
    public async Task GetAcademiesInTrustDetailsAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            TypeOfEstablishmentName = $"Academy type {n}",
            LaName = $"Local authority {n}",
            UrbanRuralName = $"UrbanRuralName {n}",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentStatusName = "Open"
        }).ToArray();
        _mockAcademiesDbContext.GiasEstablishments.AddRange(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroup, giasEstablishments);

        var result = await _sut.GetAcademiesInTrustDetailsAsync(GroupUid);

        result.Should()
            .BeEquivalentTo(giasEstablishments,
                options => options
                    .WithAutoConversion()
                    .ExcludingMissingMembers()
                    .WithMapping<AcademyDetails>(e => e.TypeOfEstablishmentName, a => a.TypeOfEstablishment)
                    .WithMapping<AcademyDetails>(e => e.LaName, a => a.LocalAuthority)
                    .WithMapping<AcademyDetails>(e => e.UrbanRuralName, a => a.UrbanRural)
            );
    }

    [Fact]
    public async Task GetAcademiesInTrustDetailsAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustDetailsAsync(GroupUid);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetNumberOfAcademiesInTrustAsync_should_return_zero_when_no_grouplinks()
    {
        var result = await _sut.GetNumberOfAcademiesInTrustAsync(GroupUid);
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task GetNumberOfAcademiesInTrustAsync_should_return_number_of_grouplinks_for_uid(int numAcademies)
    {
        _mockAcademiesDbContext.AddGiasGroupLinks("some other trust", "some other academy");
        _mockAcademiesDbContext.AddGiasGroupLinks(GroupUid, numAcademies);

        var result = await _sut.GetNumberOfAcademiesInTrustAsync(GroupUid);
        result.Should().Be(numAcademies);
    }

    [Fact]
    public async Task
        GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_null_when_multi_academy_trust()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroupForTrust("2806", groupType: "Multi-academy trust");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment(1234);
        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academy);

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().BeNull();
    }

    [Fact]
    public async Task
        GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_null_when_Federation()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroupForFederation("2806");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment(1234);
        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academy);

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().BeNull();
    }

    [Fact]
    public async Task
        GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_null_when_SAT_with_no_academies()
    {
        _ = _mockAcademiesDbContext.AddGiasGroupForTrust("2806", groupType: "Single-academy trust");

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_urn_of_SAT_academy()
    {
        var sat = _mockAcademiesDbContext.AddGiasGroupForTrust("2806", groupType: "Single-academy trust");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment(123456);
        _mockAcademiesDbContext.AddGiasGroupLinks(sat, academy);

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().Be("123456");
    }

    [Fact]
    public async Task GetAcademiesInTrustPupilNumbersAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            PhaseOfEducationName = $"Phase of Education {n}",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentStatusName = "Open",
            NumberOfPupils = $"{n}",
            SchoolCapacity = $"{n}",
            StatutoryLowAge = $"{n + 1}",
            StatutoryHighAge = $"{n + 10}"
        }).ToArray();
        _mockAcademiesDbContext.GiasEstablishments.AddRange(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroup, giasEstablishments);

        var result = await _sut.GetAcademiesInTrustPupilNumbersAsync(GroupUid);

        result.Should()
            .BeEquivalentTo(giasEstablishments,
                options => options
                    .WithAutoConversion()
                    .ExcludingMissingMembers()
            );

        for (var i = 0; i < giasEstablishments.Length; i++)
        {
            result[i].AgeRange.Minimum.Should().Be(giasEstablishments[i].StatutoryLowAge.ParseAsNullableInt());
            result[i].AgeRange.Maximum.Should().Be(giasEstablishments[i].StatutoryHighAge.ParseAsNullableInt());
        }
    }

    [Fact]
    public async Task GetAcademiesInTrustPupilNumbersAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustPupilNumbersAsync(GroupUid);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustFreeSchoolMealsAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            PhaseOfEducationName = $"Phase of Education {n}",
            TypeOfEstablishmentName = $"Type of Education {n}",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentStatusName = "Open",
            LaCode = $"{n}",
            PercentageFsm = $"{n - 950.5}"
        }).ToArray();
        _mockAcademiesDbContext.GiasEstablishments.AddRange(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroup, giasEstablishments);

        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync(GroupUid);
        result.Should()
            .BeEquivalentTo(giasEstablishments,
                options => options
                    .WithAutoConversion()
                    .ExcludingMissingMembers()
                    .WithMapping<AcademyFreeSchoolMeals>(e => e.LaCode, a => a.LocalAuthorityCode)
                    .WithMapping<AcademyFreeSchoolMeals>(e => e.PercentageFsm, a => a.PercentageFreeSchoolMeals)
            );
    }

    [Fact]
    public async Task
        GetAcademiesInTrustFreeSchoolMealsAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync(GroupUid);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetOverviewOfAcademiesInTrustAsync_should_return_academies_linked_to_trust()
    {
        // Arrange
        var giasGroup = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var giasEstablishments = Enumerable.Range(1000, 3).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            LaName = $"Local authority {n}",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentStatusName = "Open",
            NumberOfPupils = (n * 10).ToString(),
            SchoolCapacity = (n * 15).ToString()
        }).ToArray();

        _mockAcademiesDbContext.GiasEstablishments.AddRange(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroup, giasEstablishments);

        // Act
        var result = await _sut.GetOverviewOfAcademiesInTrustAsync(GroupUid);

        // Assert
        result.Should().BeEquivalentTo(giasEstablishments,
            options => options
                .WithAutoConversion()
                .ExcludingMissingMembers()
                .WithMapping<AcademyOverview>(e => e.LaName, a => a.LocalAuthority)
        );
    }

    [Fact]
    public async Task GetOverviewOfAcademiesInTrustAsync_should_handle_academies_with_missing_data()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var giasEstablishment = new GiasEstablishment
        {
            Urn = 2000,
            EstablishmentName = "Academy Missing Data",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentStatusName = "Open",
            LaName = null,
            NumberOfPupils = null,
            SchoolCapacity = null
        };
        _mockAcademiesDbContext.GiasEstablishments.Add(giasEstablishment);
        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroup, giasEstablishment);

        var result = await _sut.GetOverviewOfAcademiesInTrustAsync(GroupUid);

        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var academy = result[0];
        academy.Urn.Should().Be("2000");
        academy.LocalAuthority.Should().Be(string.Empty);
        academy.NumberOfPupils.Should().BeNull();
        academy.SchoolCapacity.Should().BeNull();
    }

    [Fact]
    public async Task GetOverviewOfAcademiesInTrustAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetOverviewOfAcademiesInTrustAsync(GroupUid);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetOverviewOfAcademiesInTrustAsync_should_return_empty_array_when_trust_does_not_exist()
    {
        var result = await _sut.GetOverviewOfAcademiesInTrustAsync("non-existent-uid");
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTrustUidFromAcademyUrnAsync_should_return_null_when_not_found()
    {
        const int unknownUrn = 999999;

        var result = await _sut.GetTrustUidFromAcademyUrnAsync(unknownUrn);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTrustUidFromAcademyUrnAsync_should_return_found_trust_uid_when_single_academy_trust()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid, groupType:"Single-academy trust");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment();
        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academy);

        var result = await _sut.GetTrustUidFromAcademyUrnAsync(academy.Urn);

        result.Should().Be(GroupUid);
    }

    [Fact]
    public async Task GetTrustUidFromAcademyUrnAsync_should_return_found_trust_uid_when_multi_academy_trust()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid, groupType:"Multi-academy trust");
        var academies =  Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}"
        }).ToArray();
        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academies);
    
        var result = await _sut.GetTrustUidFromAcademyUrnAsync(academies[0].Urn);
    
        result.Should().Be(GroupUid);
    }

    [Fact]
    public async Task GetTrustUidFromAcademyUrnAsync_should_throw_if_more_than_one_link_between_academy_and_trust()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroupForTrust(GroupUid);
        var academy = _mockAcademiesDbContext.AddGiasEstablishment();

        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academy);
        _mockAcademiesDbContext.AddGiasGroupLinks(mat, academy);

        var action = () => _sut.GetTrustUidFromAcademyUrnAsync(academy.Urn);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData("Multi-academy trust", "123")]
    [InlineData("Single-academy trust", "456")]
    [InlineData("unknown trust", null)]
    public async Task GetTrustUidFromAcademyUrnAsync_should_return_for_trust_types(string trustType, string? uid)
    {
        var trust = _mockAcademiesDbContext.AddGiasGroupForTrust(uid, groupType: trustType);
        var academy = _mockAcademiesDbContext.AddGiasEstablishment();
        _mockAcademiesDbContext.AddGiasGroupLinks(trust, academy);

        var result = await _sut.GetTrustUidFromAcademyUrnAsync(academy.Urn);

        result.Should().Be(uid);
    }
}
