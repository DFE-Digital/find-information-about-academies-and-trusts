using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
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
            new MisEstablishment { Urn = urnsAsInt[0], OverallEffectiveness = 1, InspectionEndDate = "01/01/2022" },
            new MisEstablishment { Urn = urnsAsInt[1], OverallEffectiveness = 2, InspectionEndDate = "29/02/2024" },
            new MisEstablishment { Urn = urnsAsInt[2], OverallEffectiveness = 3, InspectionEndDate = "31/12/2022" },
            new MisEstablishment { Urn = urnsAsInt[3], OverallEffectiveness = 4, InspectionEndDate = "15/10/2023" },
            new MisEstablishment { Urn = urnsAsInt[4], OverallEffectiveness = null, InspectionEndDate = null }
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[5], OverallEffectiveness = 1, LastDayOfInspection = "01/01/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[6], OverallEffectiveness = 2, LastDayOfInspection = "29/02/2024" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[7], OverallEffectiveness = 3, LastDayOfInspection = "31/12/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[8], OverallEffectiveness = 4, LastDayOfInspection = "15/10/2023" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[9], OverallEffectiveness = null, LastDayOfInspection = null }
        });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var dummyAcademyOfsted = new AcademyOfsted(string.Empty, default, default, OfstedRating.None, OfstedRating.None);

        result.Should().BeEquivalentTo(new[]
            {
                dummyAcademyOfsted with { Urn = urns[0], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Outstanding, new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[1], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Good, new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[2], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[3], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Inadequate, new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[4], CurrentOfstedRating = OfstedRating.None },
                dummyAcademyOfsted with { Urn = urns[5], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Outstanding, new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[6], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Good, new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[7], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[8], CurrentOfstedRating = new OfstedRating(OfstedRatingScore.Inadequate, new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[9], CurrentOfstedRating = OfstedRating.None }
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
            new MisEstablishment { Urn = urnsAsInt[0], PreviousFullInspectionOverallEffectiveness = "1", PreviousInspectionEndDate = "01/01/2022" },
            new MisEstablishment { Urn = urnsAsInt[1], PreviousFullInspectionOverallEffectiveness = "2", PreviousInspectionEndDate = "29/02/2024" },
            new MisEstablishment { Urn = urnsAsInt[2], PreviousFullInspectionOverallEffectiveness = "3", PreviousInspectionEndDate = "31/12/2022" },
            new MisEstablishment { Urn = urnsAsInt[3], PreviousFullInspectionOverallEffectiveness = "4", PreviousInspectionEndDate = "15/10/2023" },
            new MisEstablishment { Urn = urnsAsInt[4], PreviousFullInspectionOverallEffectiveness = null, PreviousInspectionEndDate = null }
        });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(new[]
        {
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[5], PreviousOverallEffectiveness = 1, PreviousLastDayOfInspection = "01/01/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[6], PreviousOverallEffectiveness = 2, PreviousLastDayOfInspection = "29/02/2024" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[7], PreviousOverallEffectiveness = 3, PreviousLastDayOfInspection = "31/12/2022" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[8], PreviousOverallEffectiveness = 4, PreviousLastDayOfInspection = "15/10/2023" },
            new MisFurtherEducationEstablishment { ProviderUrn = urnsAsInt[9], PreviousOverallEffectiveness = null, PreviousLastDayOfInspection = null }
        });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var dummyAcademyOfsted = new AcademyOfsted(string.Empty, default, default, OfstedRating.None, OfstedRating.None);

        result.Should().BeEquivalentTo(new[]
            {
                dummyAcademyOfsted with { Urn = urns[0], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Outstanding, new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[1], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Good, new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[2], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[3], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Inadequate, new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[4], PreviousOfstedRating = OfstedRating.None },
                dummyAcademyOfsted with { Urn = urns[5], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Outstanding, new DateTime(2022, 01, 01)) },
                dummyAcademyOfsted with { Urn = urns[6], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Good, new DateTime(2024, 02, 29)) },
                dummyAcademyOfsted with { Urn = urns[7], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.RequiresImprovement, new DateTime(2022, 12, 31)) },
                dummyAcademyOfsted with { Urn = urns[8], PreviousOfstedRating = new OfstedRating(OfstedRatingScore.Inadequate, new DateTime(2023, 10, 15)) },
                dummyAcademyOfsted with { Urn = urns[9], PreviousOfstedRating = OfstedRating.None }
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
                PreviousInspectionEndDate = "01/01/2022"
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
}
