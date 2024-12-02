using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using FluentAssertions.Execution;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class AcademyRepositoryOfstedTests
{
    private const string GroupUid = "1234";
    private readonly AcademyRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDbContext = new();
    private readonly MockLogger<AcademyRepository> _mockLogger = new();

    public AcademyRepositoryOfstedTests()
    {
        _sut = new AcademyRepository(_mockAcademiesDbContext.Object, _mockLogger.Object);
    }

    private GiasGroupLink[] AddGiasGroupLinksToMockDb(int count)
    {
        const int offset = 1000;
        var giasGroupLinks = Enumerable.Range(0, count).Select(n => new GiasGroupLink
        {
            GroupUid = GroupUid,
            Urn = $"{n + offset}",
            EstablishmentName = $"Academy {n + offset}",
            JoinedDate = "13/06/2023"
        }).ToArray();

        _mockAcademiesDbContext.AddGiasGroupLinks(giasGroupLinks);

        return giasGroupLinks;
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_return_empty_array_when_no_academies_linked_to_trust()
    {
        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_only_return_academies_linked_to_trust()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = "some other trust", Urn = "some other academy" });

        var giasGroupLinks = AddGiasGroupLinksToMockDb(6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        result.Select(a => a.Urn).Should().BeEquivalentTo(giasGroupLinks.Select(g => g.Urn));
        result.Select(a => a.EstablishmentName).Should()
            .BeEquivalentTo(giasGroupLinks.Select(g => g.EstablishmentName));
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_EstablishmentName_from_giasGroupLink()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(6);

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

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

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

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
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(987654, "15/05/2023");

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        result.Should().ContainSingle()
            .Which.CurrentOfstedRating.InspectionDate
            .Should().HaveDay(15).And.HaveMonth(5).And.HaveYear(2023);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_InspectionDate_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654, LastDayOfInspection = "15/05/2023", PreviousLastDayOfInspection = "01/02/2013" });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.InspectionDate.Should().HaveDay(15).And.HaveMonth(5).And.HaveYear(2023);
        actual.PreviousOfstedRating.InspectionDate.Should().HaveDay(1).And.HaveMonth(2).And.HaveYear(2013);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_log_error_for_each_urn_with_unknown_judgement()
    {
        //---Arrange---
        // Add each type of invalid current rating
        var invalidMisEstablishments = new List<MisEstablishment>
        {
            new() { Urn = 111111, OverallEffectiveness = "some unknown value" },
            new() { Urn = 222222, QualityOfEducation = 212 },
            new() { Urn = 333333, BehaviourAndAttitudes = 212 },
            new() { Urn = 444444, PersonalDevelopment = 212 },
            new() { Urn = 555555, EffectivenessOfLeadershipAndManagement = 212 },
            new() { Urn = 666666, EarlyYearsProvisionWhereApplicable = 212 },
            new() { Urn = 777777, SixthFormProvisionWhereApplicable = 212 },
            new() { Urn = 888888, CategoryOfConcern = "some unknown value" },
            new() { Urn = 999999, SafeguardingIsEffective = "some unknown value" }
        };
        var invalidMisFurtherEducationEstablishments = new List<MisFurtherEducationEstablishment>
        {
            new() { ProviderUrn = 101111, OverallEffectiveness = 212 },
            new() { ProviderUrn = 102222, QualityOfEducation = 212 },
            new() { ProviderUrn = 103333, BehaviourAndAttitudes = 212 },
            new() { ProviderUrn = 104444, PersonalDevelopment = 212 },
            new() { ProviderUrn = 105555, EffectivenessOfLeadershipAndManagement = 212 },
            new() { ProviderUrn = 106666, IsSafeguardingEffective = "some unknown value" }
        };

        // Add each type of invalid previous rating
        invalidMisEstablishments.AddRange(new MisEstablishment[]
        {
            new() { Urn = 111109, PreviousFullInspectionOverallEffectiveness = "some unknown value" },
            new() { Urn = 222209, PreviousQualityOfEducation = 212 },
            new() { Urn = 333309, PreviousBehaviourAndAttitudes = 212 },
            new() { Urn = 444409, PreviousPersonalDevelopment = 212 },
            new() { Urn = 555509, PreviousEffectivenessOfLeadershipAndManagement = 212 },
            new() { Urn = 666609, PreviousEarlyYearsProvisionWhereApplicable = 212 },
            new() { Urn = 777709, PreviousSixthFormProvisionWhereApplicable = "some unknown value" },
            new() { Urn = 888809, PreviousCategoryOfConcern = "some unknown value" },
            new() { Urn = 999909, PreviousSafeguardingIsEffective = "some unknown value" }
        });
        invalidMisFurtherEducationEstablishments.AddRange(new MisFurtherEducationEstablishment[]
        {
            new() { ProviderUrn = 101109, PreviousOverallEffectiveness = 212 },
            new() { ProviderUrn = 102209, PreviousQualityOfEducation = 212 },
            new() { ProviderUrn = 103309, PreviousBehaviourAndAttitudes = 212 },
            new() { ProviderUrn = 104409, PreviousPersonalDevelopment = 212 },
            new() { ProviderUrn = 105509, PreviousEffectivenessOfLeadershipAndManagement = 212 },
            new() { ProviderUrn = 106609, PreviousSafeguarding = "some unknown value" }
        });

        //Add invalid establishments to mock db
        _mockAcademiesDbContext.AddMisEstablishments(invalidMisEstablishments);
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(invalidMisFurtherEducationEstablishments);

        //Get urns of invalid entries added
        var invalidEstablishmentUrns = invalidMisEstablishments.Select(e => e.Urn!.Value)
            .Concat(invalidMisFurtherEducationEstablishments.Select(e => e.ProviderUrn))
            .ToArray();

        //Add some valid establishments to ensure we're not just logging everything
        int[] validEstablishmentUrns = [123, 456];
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment { Urn = validEstablishmentUrns[0] });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = validEstablishmentUrns[1] });

        //Create group links
        foreach (var urn in validEstablishmentUrns.Concat(invalidEstablishmentUrns))
        {
            _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
                { GroupUid = GroupUid, Urn = urn.ToString(), JoinedDate = "01/01/2022" });
        }

        //---Act---
        _ = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        //---Assert--
        //Check we got a log error for each invalid establishment
        foreach (var urn in invalidEstablishmentUrns)
        {
            _mockLogger.VerifyLogError(
                $"URN {urn} has some unrecognised ofsted ratings. This could be a data integrity issue with the Ofsted data in Academies Db.");
        }

        //Ensure that we didn't get a log error for the valid ones
        _mockLogger.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null, CategoriesOfConcern.NotInspected)]
    [InlineData("", CategoriesOfConcern.NoConcerns)]
    [InlineData("NTI", CategoriesOfConcern.NoticeToImprove)]
    [InlineData("SM", CategoriesOfConcern.SpecialMeasures)]
    [InlineData("SWK", CategoriesOfConcern.SeriousWeakness)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_CategoryOfConcern_on_current_Ofsted_from_db_when_not_further_ed(
            string? dbData,
            CategoriesOfConcern expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment { Urn = 987654, CategoryOfConcern = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

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
        GetAcademiesInTrustOfstedAsync_should_set_CategoryOfConcern_on_previous_Ofsted_from_db_when_not_further_ed(
            string? dbData,
            CategoriesOfConcern expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment
            { Urn = 987654, PreviousCategoryOfConcern = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.PreviousOfstedRating.CategoryOfConcern.Should().Be(expected);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_CategoryOfConcern_to_DoesNotApply_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.CategoryOfConcern.Should().Be(CategoriesOfConcern.DoesNotApply);
        actual.PreviousOfstedRating.CategoryOfConcern.Should().Be(CategoriesOfConcern.DoesNotApply);
    }

    [Theory]
    [InlineData(null, SafeguardingScore.NotInspected)]
    [InlineData("NULL", SafeguardingScore.NotInspected)]
    [InlineData("Yes", SafeguardingScore.Yes)]
    [InlineData("No", SafeguardingScore.No)]
    [InlineData("9", SafeguardingScore.NotRecorded)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_SafeguardingScore_on_current_Ofsted_when_non_further_ed(
            string? dbData,
            SafeguardingScore expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(
            new MisEstablishment { Urn = 987654, SafeguardingIsEffective = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.SafeguardingIsEffective.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, SafeguardingScore.NotInspected)]
    [InlineData("NULL", SafeguardingScore.NotInspected)]
    [InlineData("Yes", SafeguardingScore.Yes)]
    [InlineData("No", SafeguardingScore.No)]
    [InlineData("9", SafeguardingScore.NotRecorded)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_SafeguardingScore_on_current_Ofsted_when_further_ed(string? dbData,
            SafeguardingScore expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "876543", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(
            new MisFurtherEducationEstablishment { ProviderUrn = 876543, IsSafeguardingEffective = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.SafeguardingIsEffective.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, SafeguardingScore.NotInspected)]
    [InlineData("NULL", SafeguardingScore.NotInspected)]
    [InlineData("Yes", SafeguardingScore.Yes)]
    [InlineData("No", SafeguardingScore.No)]
    [InlineData("9", SafeguardingScore.NotRecorded)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_SafeguardingScore_on_previous_Ofsted_when_not_further_ed(
            string? dbData,
            SafeguardingScore expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment
            { Urn = 987654, PreviousSafeguardingIsEffective = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.PreviousOfstedRating.SafeguardingIsEffective.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, SafeguardingScore.NotInspected)]
    [InlineData("NULL", SafeguardingScore.NotInspected)]
    [InlineData("Yes", SafeguardingScore.Yes)]
    [InlineData("No", SafeguardingScore.No)]
    [InlineData("9", SafeguardingScore.NotRecorded)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_set_SafeguardingScore_on_previous_Ofsted_when_further_ed(string? dbData,
            SafeguardingScore expected)
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "876543", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(
            new MisFurtherEducationEstablishment { ProviderUrn = 876543, PreviousSafeguarding = dbData });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.PreviousOfstedRating.SafeguardingIsEffective.Should().Be(expected);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_handle_not_inspected_when_not_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisEstablishment(new MisEstablishment { Urn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.Should().Be(OfstedRating.NotInspected);
        actual.PreviousOfstedRating.Should().Be(OfstedRating.NotInspected);
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_handle_not_inspected_when_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishment(new MisFurtherEducationEstablishment
            { ProviderUrn = 987654 });

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var actual = result.Should().ContainSingle().Subject;
        actual.CurrentOfstedRating.Should()
            .Be(OfstedRating.NotInspected with { CategoryOfConcern = CategoriesOfConcern.DoesNotApply });
        actual.PreviousOfstedRating.Should()
            .Be(OfstedRating.NotInspected with { CategoryOfConcern = CategoriesOfConcern.DoesNotApply });
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_set_ofsted_sub_judgements_when_not_further_ed()
    {
        _mockAcademiesDbContext.AddGiasGroupLink(new GiasGroupLink
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });

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

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

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
            { GroupUid = GroupUid, Urn = "987654", JoinedDate = "01/01/2022" });

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

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

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
        var giasGroupLink = AddGiasGroupLinksToMockDb(7);
        var allUrns = giasGroupLink.Select(gl => int.Parse(gl.Urn!)).ToArray();

        // The first three urns are set up in non-further establishments with a non-further only property
        var nonFurtherUrns = allUrns.Take(3).ToArray();
        _mockAcademiesDbContext.AddMisEstablishments(
            nonFurtherUrns.Select(urn => new MisEstablishment { Urn = urn, EarlyYearsProvisionWhereApplicable = 1 }));

        // All urns are set up in further (note that this wouldn't occur in the actual db)
        _mockAcademiesDbContext.AddMisFurtherEducationEstablishments(
            allUrns.Select(urn => new MisFurtherEducationEstablishment { ProviderUrn = urn }));

        //--Act--
        var results = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        //--Assert--
        using var scope = new AssertionScope();
        // We expect the 3 urns that went to the non-further table to have early years provision
        var fromNonFurther = results.Where(ofsted => nonFurtherUrns.Contains(int.Parse(ofsted.Urn))).ToArray();
        fromNonFurther.Should().HaveCount(3);
        fromNonFurther.Should().AllSatisfy(o =>
            o.CurrentOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.Outstanding));
        // We expect the other 4 urns to have come from further ed table and so to not have early years provision
        var fromFurther = results.Except(fromNonFurther).ToArray();
        fromFurther.Should().HaveCount(4);
        fromFurther.Should().AllSatisfy(o =>
            o.CurrentOfstedRating.EarlyYearsProvision.Should().Be(OfstedRatingScore.NotInspected));
    }

    [Fact]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_log_error_and_return_ofsted_unknown_when_urn_not_found_in_mis()
    {
        var giasGroupLink = AddGiasGroupLinksToMockDb(1).Single();

        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        var academyOfsted = result.Should().ContainSingle().Which;
        academyOfsted.Urn.Should().Be(giasGroupLink.Urn);
        academyOfsted.CurrentOfstedRating.Should().Be(OfstedRating.Unknown);
        academyOfsted.PreviousOfstedRating.Should().Be(OfstedRating.Unknown);

        _mockLogger.VerifyLogError(
            $"URN {giasGroupLink.Urn} was not found in Mis.Establishments or Mis.FurtherEducationEstablishments. This indicates a data integrity issue with the Ofsted data in Academies Db.");
    }

    [Fact]
    public async Task GetAcademiesInTrustOfstedAsync_should_not_log_error_when_all_establishments_exist_and_are_valid()
    {
        var giasGroupLinks = AddGiasGroupLinksToMockDb(2);
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

        await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public void ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_NotInspected_When_Rating_Is_Null()
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(null);

        // Assert
        result.Should().Be(OfstedRatingScore.NotInspected);
    }

    [Theory]
    [InlineData("Not judged")]
    [InlineData("not judged")]
    [InlineData("NOT JUDGED")]
    [InlineData("NoT JuDgEd")]
    public void
        ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_NoJudgement_When_Rating_Is_NotJudged_CaseInsensitive(
            string rating)
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
    [InlineData("-1", OfstedRatingScore.NotInspected)]
    public void
        ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_Correct_OfstedRatingScore_When_Rating_Is_Valid_Integer_String(
            string rating, OfstedRatingScore expected)
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
    public void
        ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_Unknown_When_Rating_Is_Integer_Not_Defined_In_Enum(
            string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.Unknown);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("Good")]
    [InlineData("Outstanding")]
    [InlineData("Requires Improvement")]
    [InlineData("Inadequate")]
    [InlineData("N/A")]
    [InlineData("Unknown")]
    public void ConvertOverallEffectivenessToOfstedRatingScore_Should_Return_Unknown_When_Rating_Is_Invalid_String(
        string rating)
    {
        // Act
        var result = AcademyRepository.ConvertOverallEffectivenessToOfstedRatingScore(rating);

        // Assert
        result.Should().Be(OfstedRatingScore.Unknown);
    }

    [Theory]
    [InlineData("1", "2", OfstedRatingScore.Outstanding, OfstedRatingScore.Good)]
    [InlineData("Not judged", "Not judged", OfstedRatingScore.NoJudgement, OfstedRatingScore.NoJudgement)]
    [InlineData(null, null, OfstedRatingScore.NotInspected, OfstedRatingScore.NotInspected)]
    [InlineData("abc", "def", OfstedRatingScore.Unknown, OfstedRatingScore.Unknown)]
    [InlineData("2", "Not judged", OfstedRatingScore.Good, OfstedRatingScore.NoJudgement)]
    [InlineData("Not judged", "1", OfstedRatingScore.NoJudgement, OfstedRatingScore.Outstanding)]
    public async Task
        GetAcademiesInTrustOfstedAsync_should_correctly_convert_OverallEffectiveness_and_PreviousFullInspectionOverallEffectiveness(
            string? overallEffectiveness,
            string? previousOverallEffectiveness,
            OfstedRatingScore expectedCurrentScore,
            OfstedRatingScore expectedPreviousScore)
    {
        // Arrange
        var giasGroupLink = AddGiasGroupLinksToMockDb(1).Single();
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
        var result = await _sut.GetAcademiesInTrustOfstedAsync(GroupUid);

        // Assert
        var academyOfsted = result.Should().ContainSingle().Which;
        academyOfsted.Urn.Should().Be(giasGroupLink.Urn);
        academyOfsted.CurrentOfstedRating.OverallEffectiveness.Should().Be(expectedCurrentScore);
        academyOfsted.PreviousOfstedRating.OverallEffectiveness.Should().Be(expectedPreviousScore);
    }
}
