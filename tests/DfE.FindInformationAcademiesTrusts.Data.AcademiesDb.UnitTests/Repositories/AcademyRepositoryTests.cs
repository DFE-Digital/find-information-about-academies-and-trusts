using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using FluentAssertions.Execution;

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

        var giasGroupLinks = AddGiasGroupLinksToMockDb("1234", 6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.Urn).Should().BeEquivalentTo(giasGroupLinks.Select(g => g.Urn));
        result.Select(a => a.EstablishmentName).Should()
            .BeEquivalentTo(giasGroupLinks.Select(g => g.EstablishmentName));
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_EstablishmentName_from_giasGroupLink()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb("1234", 6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.EstablishmentName).Should()
            .BeEquivalentTo(giasGroupLinks.Select(g => g.EstablishmentName));
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_DateAcademyJoinedTrust_from_giasGroupLink()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb("1234", 3);
        giasGroupLinks[0].JoinedDate = "01/01/2022";
        giasGroupLinks[1].JoinedDate = "29/02/2024";
        giasGroupLinks[2].JoinedDate = "31/12/1999";

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Select(a => a.DateAcademyJoinedTrust)
            .Should()
            .BeEquivalentTo([
                new DateTime(2022, 01, 01),
                new DateTime(2024, 02, 29),
                new DateTime(1999, 12, 31)
            ]);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_InspectionDate_when_not_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(987654, "15/05/2023");

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Should().ContainSingle()
            .Which.CurrentOfstedRating.InspectionDate
            .Should().HaveDay(15).And.HaveMonth(5).And.HaveYear(2023);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_InspectionDate_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654, LastDayOfInspection = "15/05/2023", PreviousLastDayOfInspection = "01/02/2013" });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.InspectionDate.Should().HaveDay(15).And.HaveMonth(5).And.HaveYear(2023);
        actual.PreviousOfstedRating.InspectionDate.Should().HaveDay(1).And.HaveMonth(2).And.HaveYear(2013);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_log_error_on_unknown_CategoryOfConcern()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment
            { Urn = 987654, CategoryOfConcern = "some unknown value" });

        _ = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        _mockLogger.VerifyLogError(
            "Category of concern some unknown value was not recognised. This could be a data integrity issue with the Ofsted data in Academies Db.");
    }

    [Theory]
    [InlineData(null, CategoriesOfConcern.NotInspected)]
    [InlineData("", CategoriesOfConcern.NoConcerns)]
    [InlineData("NTI", CategoriesOfConcern.NoticeToImprove)]
    [InlineData("SM", CategoriesOfConcern.SpecialMeasures)]
    [InlineData("SWK", CategoriesOfConcern.SeriousWeakness)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_concern_on_current_Ofsted_from_db_when_not_further_ed(string? dbData,
            CategoriesOfConcern expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment { Urn = 987654, CategoryOfConcern = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        result.Should().ContainSingle()
            .Which.CurrentOfstedRating.CategoryOfConcern.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, CategoriesOfConcern.NotInspected)]
    [InlineData("", CategoriesOfConcern.NoConcerns)]
    [InlineData("NTI", CategoriesOfConcern.NoticeToImprove)]
    [InlineData("SM", CategoriesOfConcern.SpecialMeasures)]
    [InlineData("SWK", CategoriesOfConcern.SeriousWeakness)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_concern_on_previous_Ofsted_from_db_when_not_further_ed(string? dbData,
            CategoriesOfConcern expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment
            { Urn = 987654, PreviousCategoryOfConcern = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.PreviousOfstedRating.CategoryOfConcern.Should().Be(expected);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_be_does_not_apply_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.CategoryOfConcern.Should().Be(CategoriesOfConcern.DoesNotApply);
        actual.PreviousOfstedRating.CategoryOfConcern.Should().Be(CategoriesOfConcern.DoesNotApply);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_handle_not_inspected_when_not_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment { Urn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.Should().Be(OfstedRating.None);
        actual.PreviousOfstedRating.Should().Be(OfstedRating.None);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_handle_not_inspected_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.Should()
            .Be(OfstedRating.None with { CategoryOfConcern = CategoriesOfConcern.DoesNotApply });
        actual.PreviousOfstedRating.Should()
            .Be(OfstedRating.None with { CategoryOfConcern = CategoriesOfConcern.DoesNotApply });
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_ofsted_sub_judgements_when_not_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });

        _mockAcademiesDbContext.AddMisEstablishment(
            new MisEstablishment
            {
                Urn = 987654,

                QualityOfEducation = 1,
                BehaviourAndAttitudes = 2,
                PersonalDevelopment = 3,
                EffectivenessOfLeadershipAndManagement = 4,
                EarlyYearsProvisionWhereApplicable = 1,
                SixthFormProvisionWhereApplicable = 2,

                PreviousQualityOfEducation = 3,
                PreviousBehaviourAndAttitudes = 4,
                PreviousPersonalDevelopment = 1,
                PreviousEffectivenessOfLeadershipAndManagement = 2,
                PreviousEarlyYearsProvisionWhereApplicable = 3,
                PreviousSixthFormProvisionWhereApplicable = "4"
            }
        );

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;

        actual.CurrentOfstedRating.QualityOfEducation.Should().Be(OfstedRatingScore.Outstanding);
        actual.CurrentOfstedRating.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.Good);
        actual.CurrentOfstedRating.PersonalDevelopment.Should().Be(OfstedRatingScore.RequiresImprovement);
        actual.CurrentOfstedRating.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.Inadequate);
        actual.CurrentOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.Outstanding);
        actual.CurrentOfstedRating.SixthFormProvision.Should().Be(OfstedRatingScore.Good);

        actual.PreviousOfstedRating.QualityOfEducation.Should().Be(OfstedRatingScore.RequiresImprovement);
        actual.PreviousOfstedRating.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.Inadequate);
        actual.PreviousOfstedRating.PersonalDevelopment.Should().Be(OfstedRatingScore.Outstanding);
        actual.PreviousOfstedRating.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.Good);
        actual.PreviousOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.RequiresImprovement);
        actual.PreviousOfstedRating.SixthFormProvision.Should().Be(OfstedRatingScore.Inadequate);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_ofsted_judgements_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "1234", Urn = "987654", JoinedDate = "01/01/2022" });

        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(
            new MisFurtherEducationEstablishment
            {
                ProviderUrn = 987654,

                OverallEffectiveness = 1,
                QualityOfEducation = 2,
                BehaviourAndAttitudes = 3,
                PersonalDevelopment = 4,
                EffectivenessOfLeadershipAndManagement = 1,

                PreviousOverallEffectiveness = 2,
                PreviousQualityOfEducation = 3,
                PreviousBehaviourAndAttitudes = 4,
                PreviousPersonalDevelopment = 1,
                PreviousEffectivenessOfLeadershipAndManagement = 2
            }
        );

        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.OverallEffectiveness.Should().Be(OfstedRatingScore.Outstanding);
        actual.CurrentOfstedRating.QualityOfEducation.Should().Be(OfstedRatingScore.Good);
        actual.CurrentOfstedRating.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.RequiresImprovement);
        actual.CurrentOfstedRating.PersonalDevelopment.Should().Be(OfstedRatingScore.Inadequate);
        actual.CurrentOfstedRating.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.Outstanding);

        actual.PreviousOfstedRating.OverallEffectiveness.Should().Be(OfstedRatingScore.Good);
        actual.PreviousOfstedRating.QualityOfEducation.Should().Be(OfstedRatingScore.RequiresImprovement);
        actual.PreviousOfstedRating.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.Inadequate);
        actual.PreviousOfstedRating.PersonalDevelopment.Should().Be(OfstedRatingScore.Outstanding);
        actual.PreviousOfstedRating.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.Good);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_only_query_further_ed_for_urns_not_in_mis_establishments()
    {
        //--Arrange--
        var giasGroupLink = AddGiasGroupLinksToMockDb("1234", 7);
        var allUrns = giasGroupLink.Select(gl => int.Parse(gl.Urn!)).ToArray();

        // The first three urns are set up in non-further establishments with a non-further only property
        var nonFurtherUrns = allUrns.Take(3).ToArray();
        _mockAcademiesDbContext.AddMisEstablishments(
            nonFurtherUrns.Select(urn => new MisEstablishment { Urn = urn, EarlyYearsProvisionWhereApplicable = 1 }));

        // All urns are set up in further (note that this wouldn't occur in the actual db)
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(
            allUrns.Select(urn => new MisFurtherEducationEstablishment { ProviderUrn = urn }));

        //--Act--
        var results = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        //--Assert--
        using var scope = new AssertionScope();
        // We expect the 3 urns that went to the non-further table to have early years provision
        var fromNonFurther = results.Where(ofsted => nonFurtherUrns.Contains(int.Parse(ofsted.Urn))).ToArray();
        fromNonFurther.Should().HaveCount(3);
        fromNonFurther.Should().AllSatisfy(o =>
            o.CurrentOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.Outstanding));
        // We expect the other 4 urns to have come from non-further table and so to not have early years provision
        var fromFurther = results.Except(fromNonFurther).ToArray();
        fromFurther.Should().HaveCount(4);
        fromFurther.Should().AllSatisfy(o =>
            o.CurrentOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.None));
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_log_error_and_return_ofsted_none_when_urn_not_found_in_mis()
    {
        var giasGroupLink = AddGiasGroupLinksToMockDb("1234", 1).Single();

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
        var giasGroupLinks = AddGiasGroupLinksToMockDb("1234", 2);
        var urns = giasGroupLinks.Select(gl => int.Parse(gl.Urn!)).ToArray();

        _mockAcademiesDbContext.AddMisEstablishment(
            new MisEstablishment
            {
                Urn = urns[0],
                PreviousFullInspectionOverallEffectiveness = "1",
                PreviousInspectionStartDate = "01/01/2022"
            }
        );
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(
            new MisFurtherEducationEstablishment
            {
                ProviderUrn = urns[1],
                PreviousOverallEffectiveness = 1,
                PreviousLastDayOfInspection = "01/01/2022"
            }
        );

        await _sut.GetAcademiesInTrustOfstedAsync("1234");

        _mockLogger.VerifyNoOtherCalls();
    }

    private GiasGroupLink[] AddGiasGroupLinksToMockDb(string? groupUid, int count, int offset = 1000)
    {
        var giasGroupLinks = Enumerable.Range(0, count).Select(n => new GiasGroupLink
        {
            GroupUid = groupUid,
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
        _mockAcademiesDbContext.LinkGiasEstablishmentsToGiasGroup([academy], mat);

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
        _mockAcademiesDbContext.LinkGiasEstablishmentsToGiasGroup([academy], sat);

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
    public async Task GetOverviewOfAcademiesInTrustAsync_should_return_academies_linked_to_trust()
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
        var result = await _sut.GetOverviewOfAcademiesInTrustAsync("1234");

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
        var giasGroup = _mockAcademiesDbContext.AddGiasGroup("1234");
        var giasEstablishment = new GiasEstablishment
        {
            Urn = 2000,
            EstablishmentName = "Academy Missing Data",
            LaName = null,
            NumberOfPupils = null,
            SchoolCapacity = null
        };
        _mockAcademiesDbContext.AddGiasEstablishments([giasEstablishment]);
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
        {
            GroupUid = giasGroup.GroupUid,
            Urn = giasEstablishment.Urn.ToString()
        });

        var result = await _sut.GetOverviewOfAcademiesInTrustAsync("1234");

        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var academy = result.First();
        academy.Urn.Should().Be("2000");
        academy.LocalAuthority.Should().Be(string.Empty);
        academy.NumberOfPupils.Should().BeNull();
        academy.SchoolCapacity.Should().BeNull();
    }

    [Fact]
    public async Task GetOverviewOfAcademiesInTrustAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetOverviewOfAcademiesInTrustAsync("1234");
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

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_None_When_Rating_Is_NullOrWhitespace(
        string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.None);
    }

    [Theory]
    [InlineData("Not judged")]
    [InlineData("not judged")]
    [InlineData("NOT JUDGED")]
    [InlineData("NoT JuDgEd")]
    public void Should_Return_NoJudgement_When_Rating_Is_NotJudged_CaseInsensitive(string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.NoJudgement);
    }

    [Theory]
    [InlineData("1", OfstedRatingScore.Outstanding)]
    [InlineData("2", OfstedRatingScore.Good)]
    [InlineData("3", OfstedRatingScore.RequiresImprovement)]
    [InlineData("4", OfstedRatingScore.Inadequate)]
    [InlineData("8", OfstedRatingScore.DoesNotApply)]
    [InlineData("9", OfstedRatingScore.NoJudgement)]
    [InlineData("0", OfstedRatingScore.InsufficientEvidence)]
    [InlineData("-1", OfstedRatingScore.None)]
    public void Should_Return_Correct_OfstedRatingScore_When_Rating_Is_Valid_Integer_String(string rating,
        OfstedRatingScore expected)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("5")]
    [InlineData("10")]
    [InlineData("-2")]
    public void Should_Return_None_When_Rating_Is_Integer_Not_Defined_In_Enum(string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.None);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("Good")]
    [InlineData("Outstanding")]
    [InlineData("Requires Improvement")]
    [InlineData("Inadequate")]
    [InlineData("N/A")]
    [InlineData("Unknown")]
    public void Should_Return_None_When_Rating_Is_Invalid_String(string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.None);
    }

    [Theory]
    [InlineData("1", "2", OfstedRatingScore.Outstanding, OfstedRatingScore.Good)]
    [InlineData("Not judged", "Not judged", OfstedRatingScore.NoJudgement, OfstedRatingScore.NoJudgement)]
    [InlineData("abc", "def", OfstedRatingScore.None, OfstedRatingScore.None)]
    [InlineData("2", "Not judged", OfstedRatingScore.Good, OfstedRatingScore.NoJudgement)]
    [InlineData("Not judged", "1", OfstedRatingScore.NoJudgement, OfstedRatingScore.Outstanding)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_correctly_convert_OverallEffectiveness_and_PreviousFullInspectionOverallEffectiveness(
            string overallEffectiveness,
            string previousOverallEffectiveness,
            OfstedRatingScore expectedCurrentScore,
            OfstedRatingScore expectedPreviousScore)
    {
        // Arrange
        var giasGroupLink = AddGiasGroupLinksToMockDb("1234", 1).Single();
        var urn = int.Parse(giasGroupLink.Urn!);

        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment
        {
            Urn = urn,
            OverallEffectiveness = overallEffectiveness,
            PreviousFullInspectionOverallEffectiveness = previousOverallEffectiveness,
            InspectionStartDate = "01/01/2022",
            PreviousInspectionStartDate = "01/01/2021"
        });

        // Act
        var result = await _sut.GetAcademiesInTrustOfstedAsync("1234");

        // Assert
        var academyOfsted = result.Should().ContainSingle().Which;
        academyOfsted.Urn.Should().Be(giasGroupLink.Urn);
        academyOfsted.CurrentOfstedRating.OverallEffectiveness.Should().Be(expectedCurrentScore);
        academyOfsted.PreviousOfstedRating.OverallEffectiveness.Should().Be(expectedPreviousScore);
    }
}
