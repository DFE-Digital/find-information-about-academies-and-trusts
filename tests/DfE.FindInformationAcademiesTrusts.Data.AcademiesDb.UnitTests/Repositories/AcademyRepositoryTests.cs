using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factory;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class AcademyRepositoryTests
{
    private readonly AcademyRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly MockLogger<AcademyRepository> _mockLogger = new();

    public AcademyRepositoryTests()
    {
        _sut = new AcademyRepository(_mockAcademiesDbContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAcademiesInTrustDetailsAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            TypeOfEstablishmentName = $"Academy type {n}",
            LaName = $"Local authority {n}",
            UrbanRuralName = $"UrbanRuralName {n}"
        }).ToArray();
        _mockAcademiesDbContext.AddGiasEstablishments(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(giasEstablishments, giasGroup);

        var result = await _sut.GetAcademiesInTrustDetailsAsync("1234");

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
        var result = await _sut.GetAcademiesInTrustDetailsAsync("1234");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_only_return_academies_linked_to_trust()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "some other trust", Urn = "some other academy" });

        var giasGroupLinks = AddGiasGroupLinksToMockDb(6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.Urn).Should().BeEquivalentTo(giasGroupLinks.Select(g => g.Urn));
        result.Select(a => a.EstablishmentName).Should()
            .BeEquivalentTo(giasGroupLinks.Select(g => g.EstablishmentName));
    }


    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_EstablishmentName_from_giasGroupLink()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.EstablishmentName).Should()
            .BeEquivalentTo(giasGroupLinks.Select(g => g.EstablishmentName));
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_DateAcademyJoinedTrust_from_giasGroupLink()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(3);
        giasGroupLinks[0].JoinedDate = "01/01/2022";
        giasGroupLinks[1].JoinedDate = "29/02/2024";
        giasGroupLinks[2].JoinedDate = "31/12/1999";

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.DateAcademyJoinedTrust)
            .Should()
            .BeEquivalentTo(new[]
            {
                new DateTime(2022, 01, 01),
                new DateTime(2024, 02, 29),
                new DateTime(1999, 12, 31)
            });
    }

    // @formatter:max_line_length 500 - these tests are very difficult to read after line wrapping is enforced so increase max line length
    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_current_ofsted()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(10);
        var urns = giasGroupLinks.Select(gl => gl.Urn!).ToArray();
        var urnsAsInt = urns.Select(int.Parse).ToArray();

        _mockAcademiesDbContext.AddMisEstablishments(new[]
        {
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[0], 1, null, SafeguardingScoreString.Yes, "01/01/2022"),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[1], 2, CategoriesOfConcernString.NoticeToImprove, null, "29/02/2024"),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[2], 3, CategoriesOfConcernString.SpecialMeasures, SafeguardingScoreString.Nine, "31/12/2022"),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[3], 4, CategoriesOfConcernString.SeriousWeakness, SafeguardingScoreString.No, "15/10/2023"),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[4], null, null, null, null)
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[5], 1, SafeguardingScoreString.Yes, "01/01/2022"),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[6], 2, null, "29/02/2024"),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[7], 3, SafeguardingScoreString.Nine, "31/12/2022"),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[8], 4, SafeguardingScoreString.No, "15/10/2023"),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[9], null, null, null)
        });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var dummyAcademyOfsted = new AcademyOfsted(string.Empty, default, default, OfstedRating.None, OfstedRating.None);

        result.Should().BeEquivalentTo(new[]
            {
                dummyAcademyOfsted with { Urn = urns[0], CurrentOfstedRating = OfstedResultFactory.OutstandingEstablishmentOfstedRating(new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[1], CurrentOfstedRating = OfstedResultFactory.GoodEstablishmentOfstedRating(new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[2], CurrentOfstedRating = OfstedResultFactory.RequiresImprovementEstablishmentOfstedRating(new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[3], CurrentOfstedRating = OfstedResultFactory.InadequateEstablishmentOfstedRating(new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[4], CurrentOfstedRating = OfstedResultFactory.NoneEstablishmentOfstedRating(null) },
                dummyAcademyOfsted with { Urn = urns[5], CurrentOfstedRating = OfstedResultFactory.OutstandingFurtherEstablishmentOfstedRating(new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[6], CurrentOfstedRating = OfstedResultFactory.GoodFurtherEstablishmentOfstedRating(new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[7], CurrentOfstedRating = OfstedResultFactory.RequiresFurtherImprovementEstablishmentOfstedRating(new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[8], CurrentOfstedRating = OfstedResultFactory.InadequateFurtherEstablishmentOfstedRating(new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[9], CurrentOfstedRating = OfstedResultFactory.NoneFurtherEstablishmentOfstedRating(null) }
            },
            options => options
                .Including(a => a.Urn)
                .Including(a => a.CurrentOfstedRating)
        );
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_previous_ofsted()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(10);
        var urns = giasGroupLinks.Select(gl => gl.Urn!).ToArray();
        var urnsAsInt = urns.Select(int.Parse).ToArray();

        _mockAcademiesDbContext.AddMisEstablishments(new[]
        {
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[0], 1, null, SafeguardingScoreString.Yes, "01/01/2022", true),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[1], 2, CategoriesOfConcernString.NoticeToImprove, null, "29/02/2024", true),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[2], 3, CategoriesOfConcernString.SpecialMeasures, SafeguardingScoreString.Nine, "31/12/2022", true),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[3], 4, CategoriesOfConcernString.SeriousWeakness, SafeguardingScoreString.No, "15/10/2023", true),
            MisEstablishmentFactory.CreateMisEstablishment(urnsAsInt[4], null, null, null, null, true)
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[5], 1, SafeguardingScoreString.Yes, "01/01/2022", true),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[6], 2, null, "29/02/2024", true),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[7], 3, SafeguardingScoreString.Nine, "31/12/2022", true),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[8], 4, SafeguardingScoreString.No, "15/10/2023", true),
            MisFurtherEstablishmentFactory.CreateMisFurtherEducationEstablishment(urnsAsInt[9], null, null, null, true)
        });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var dummyAcademyOfsted = new AcademyOfsted(string.Empty, default, default, OfstedRating.None, OfstedRating.None);

        result.Should().BeEquivalentTo(new[]
            {
                dummyAcademyOfsted with { Urn = urns[0], PreviousOfstedRating = OfstedResultFactory.OutstandingEstablishmentOfstedRating(new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[1], PreviousOfstedRating = OfstedResultFactory.GoodEstablishmentOfstedRating(new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[2], PreviousOfstedRating = OfstedResultFactory.RequiresImprovementEstablishmentOfstedRating(new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[3], PreviousOfstedRating = OfstedResultFactory.InadequateEstablishmentOfstedRating(new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[4], PreviousOfstedRating = OfstedResultFactory.NoneEstablishmentOfstedRating(null) },
                dummyAcademyOfsted with { Urn = urns[5], PreviousOfstedRating = OfstedResultFactory.OutstandingFurtherEstablishmentOfstedRating(new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[6], PreviousOfstedRating = OfstedResultFactory.GoodFurtherEstablishmentOfstedRating(new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[7], PreviousOfstedRating = OfstedResultFactory.RequiresFurtherImprovementEstablishmentOfstedRating(new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[8], PreviousOfstedRating = OfstedResultFactory.InadequateFurtherEstablishmentOfstedRating(new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[9], PreviousOfstedRating = OfstedResultFactory.NoneFurtherEstablishmentOfstedRating(null) }
            },
            options => options
                .Including(a => a.Urn)
                .Including(a => a.PreviousOfstedRating)
        );
    }
    // @formatter:max_line_length restore

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_log_error_and_return_ofsted_none_when_urn_not_found_in_mis()
    {
        var giasGroupLink = AddGiasGroupLinksToMockDb(1).Single();

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var academyOfsted = result.Should().ContainSingle().Which;
        academyOfsted.Urn.Should().Be(giasGroupLink.Urn);
        academyOfsted.CurrentOfstedRating.Should().Be(OfstedRating.None);
        academyOfsted.PreviousOfstedRating.Should().Be(OfstedRating.None);

        _mockLogger.VerifyLogError(
            $"URN {giasGroupLink.Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.");
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_not_log_error_when_urns_are_all_found_in_mis()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(2);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn!)).ToArray();

        _mockAcademiesDbContext.AddMisEstablishments(new[]
        {
            new MisEstablishment
            {
                Urn = urns[0],
                PreviousFullInspectionOverallEffectiveness = "1",
                PreviousInspectionStartDate = "01/01/2022"
            }
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            new MisFurtherEducationEstablishment
            {
                ProviderUrn = urns[1],
                PreviousOverallEffectiveness = 1,
                PreviousLastDayOfInspection = "01/01/2022"
            }
        });

        await _sut.GetAcademiesInTrustOfstedAsync("1234");

        _mockLogger.VerifyNoOtherCalls();
    }

    private GiasGroupLink[] AddGiasGroupLinksToMockDb(int count, int offset = 1000)
    {
        var giasGroupLinks = Enumerable.Range(0, count).Select(n => new GiasGroupLink
        {
            GroupUid = "1234",
            Urn = $"{n + offset}",
            EstablishmentName = $"Academy {n + offset}",
            JoinedDate = "13/06/2023"
        }).ToArray();

        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroupLinks);

        return giasGroupLinks;
    }

    [Fact]
    public async Task GetNumberOfAcademiesInTrustAsync_should_return_zero_when_no_grouplinks()
    {
        var result = await _sut.GetNumberOfAcademiesInTrustAsync("1234");
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task GetNumberOfAcademiesInTrustAsync_should_return_number_of_grouplinks_for_uid(int numAcademies)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "some other trust", Urn = "some other academy" });

        for (var i = 0; i < numAcademies; i++)
        {
            _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink { GroupUid = "1234", Urn = $"{i}" });
        }

        var result = await _sut.GetNumberOfAcademiesInTrustAsync("1234");
        result.Should().Be(numAcademies);
    }

    [Fact]
    public async Task
        GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_null_when_multi_academy_trust()
    {
        var mat = _mockAcademiesDbContext.AddGiasGroup("2806", groupType: "Multi-academy trust");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment(1234);
        _mockAcademiesDbContext.LinkGiasEstablishmentsToGiasGroup(new[] { academy }, mat);

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().BeNull();
    }

    [Fact]
    public async Task
        GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_null_when_SAT_with_no_academies()
    {
        _ = _mockAcademiesDbContext.AddGiasGroup("2806", groupType: "Single-academy trust");

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUrnForSingleAcademyTrustAsync_should_set_singleAcademyTrustAcademyUrn_to_urn_of_SAT_academy()
    {
        var sat = _mockAcademiesDbContext.AddGiasGroup("2806", groupType: "Single-academy trust");
        var academy = _mockAcademiesDbContext.AddGiasEstablishment(1234);
        _mockAcademiesDbContext.LinkGiasEstablishmentsToGiasGroup(new[] { academy }, sat);

        var result = await _sut.GetSingleAcademyTrustAcademyUrnAsync("2806");

        result.Should().Be("1234");
    }

    [Fact]
    public async Task GetAcademiesInTrustPupilNumbersAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            PhaseOfEducationName = $"Phase of Education {n}",
            NumberOfPupils = $"{n}",
            SchoolCapacity = $"{n}",
            StatutoryLowAge = $"{n + 1}",
            StatutoryHighAge = $"{n + 10}"
        }).ToArray();
        _mockAcademiesDbContext.AddGiasEstablishments(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(giasEstablishments, giasGroup);

        var result = await _sut.GetAcademiesInTrustPupilNumbersAsync("1234");

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
        var result = await _sut.GetAcademiesInTrustPupilNumbersAsync("1234");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustFreeSchoolMealsAsync_should_return_academies_linked_to_trust()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishments = Enumerable.Range(1000, 6).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            PhaseOfEducationName = $"Phase of Education {n}",
            TypeOfEstablishmentName = $"Type of Education {n}",
            LaCode = $"{n}",
            PercentageFsm = $"{n - 950.5}"
        }).ToArray();
        _mockAcademiesDbContext.AddGiasEstablishments(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(giasEstablishments, giasGroup);

        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync("1234");
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
        var result = await _sut.GetAcademiesInTrustFreeSchoolMealsAsync("1234");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustOverviewAsync_should_return_academies_linked_to_trust()
    {
        // Arrange
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishments = Enumerable.Range(1000, 3).Select(n => new GiasEstablishment
        {
            Urn = n,
            EstablishmentName = $"Academy {n}",
            LaName = $"Local authority {n}",
            NumberOfPupils = (n * 10).ToString(),
            SchoolCapacity = (n * 15).ToString()
        }).ToArray();

        _mockAcademiesDbContext.AddGiasEstablishments(giasEstablishments);
        _mockAcademiesDbContext.AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(giasEstablishments, giasGroup);

        // Act
        var result = await _sut.GetAcademiesInTrustOverviewAsync("1234");

        // Assert
        result.Should().BeEquivalentTo(giasEstablishments,
            options => options
                .WithAutoConversion()
                .ExcludingMissingMembers()
                .WithMapping<AcademyOverview>(e => e.LaName, a => a.LocalAuthority)
        );
    }

    // @formatter:max_line_length 500 - these tests are very difficult to read after line wrapping is enforced so increase max line length
    [Fact]
    public async Task GetAcademiesInTrustOverviewAsync_should_set_current_ofsted()
    {
        // Arrange
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishments = _mockAcademiesDbContext.AddGiasEstablishments(10);
        _mockAcademiesDbContext.AddGiasGroupLinksForGiasEstablishmentsToGiasGroup(giasEstablishments, giasGroup);

        var urnsAsInt = giasEstablishments.Select(gl => gl.Urn).ToArray();
        var urns = urnsAsInt.Select(u => u.ToString()).ToArray();

        _mockAcademiesDbContext.AddMisEstablishments(new[]
        {
            new MisEstablishment { Urn = urnsAsInt[0], OverallEffectiveness = 1, InspectionStartDate = "01/01/2022" },
            new MisEstablishment { Urn = urnsAsInt[1], OverallEffectiveness = 2, InspectionStartDate = "29/02/2024" },
            new MisEstablishment { Urn = urnsAsInt[2], OverallEffectiveness = 3, InspectionStartDate = "31/12/2022" },
            new MisEstablishment { Urn = urnsAsInt[3], OverallEffectiveness = 4, InspectionStartDate = "15/10/2023" },
            new MisEstablishment { Urn = urnsAsInt[4], OverallEffectiveness = null, InspectionStartDate = null }
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[5], OverallEffectiveness = 1, LastDayOfInspection = "01/01/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[6], OverallEffectiveness = 2, LastDayOfInspection = "29/02/2024" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[7], OverallEffectiveness = 3, LastDayOfInspection = "31/12/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[8], OverallEffectiveness = 4, LastDayOfInspection = "15/10/2023" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[9], OverallEffectiveness = null, LastDayOfInspection = null }
        });

        // Act
        var result = await _sut.GetAcademiesInTrustOverviewAsync("1234");

        // Assert
        var dummyAcademyOverview = new AcademyOverview(string.Empty, string.Empty, null, null, OfstedRatingScore.None);

        result.Should().BeEquivalentTo(new[]
            {
                dummyAcademyOverview with { Urn = urns[0], CurrentOfstedRating = OfstedRatingScore.Outstanding },
                dummyAcademyOverview with { Urn = urns[1], CurrentOfstedRating = OfstedRatingScore.Good },
                dummyAcademyOverview with { Urn = urns[2], CurrentOfstedRating = OfstedRatingScore.RequiresImprovement },
                dummyAcademyOverview with { Urn = urns[3], CurrentOfstedRating = OfstedRatingScore.Inadequate },
                dummyAcademyOverview with { Urn = urns[4], CurrentOfstedRating = OfstedRatingScore.None },
                dummyAcademyOverview with { Urn = urns[5], CurrentOfstedRating = OfstedRatingScore.Outstanding },
                dummyAcademyOverview with { Urn = urns[6], CurrentOfstedRating = OfstedRatingScore.Good },
                dummyAcademyOverview with { Urn = urns[7], CurrentOfstedRating = OfstedRatingScore.RequiresImprovement },
                dummyAcademyOverview with { Urn = urns[8], CurrentOfstedRating = OfstedRatingScore.Inadequate },
                dummyAcademyOverview with { Urn = urns[9], CurrentOfstedRating = OfstedRatingScore.None }
            },
            options => options
                .Including(a => a.Urn)
                .Including(a => a.CurrentOfstedRating)
        );
    }
    // @formatter:max_line_length restore

    [Fact]
    public async Task GetAcademiesInTrustOverviewAsync_should_handle_academies_with_missing_data()
    {
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishment = new GiasEstablishment
        {
            Urn = 2000,
            EstablishmentName = "Academy Missing Data",
            LaName = null,
            NumberOfPupils = null,
            SchoolCapacity = null
        };
        _mockAcademiesDbContext.AddGiasEstablishments(new[] { giasEstablishment });
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
        {
            GroupUid = giasGroup.GroupUid,
            Urn = giasEstablishment.Urn.ToString()
        });

        var result = await _sut.GetAcademiesInTrustOverviewAsync("1234");

        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var academy = result.First();
        academy.Urn.Should().Be("2000");
        academy.LocalAuthority.Should().Be(string.Empty);
        academy.NumberOfPupils.Should().BeNull();
        academy.SchoolCapacity.Should().BeNull();
    }

    [Fact]
    public async Task GetAcademiesInTrustOverviewAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustOverviewAsync("1234");
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustOverviewAsync_should_return_empty_array_when_trust_does_not_exist()
    {
        var result = await _sut.GetAcademiesInTrustOverviewAsync("non-existent-uid");
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
