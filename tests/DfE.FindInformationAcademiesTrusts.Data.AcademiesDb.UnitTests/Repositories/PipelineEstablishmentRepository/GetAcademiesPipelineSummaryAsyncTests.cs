using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories.PipelineEstablishmentRepository;

public class GetAcademiesPipelineSummaryAsyncTests
{
    private readonly MockAcademiesDbContext _mockContext = new();
    private readonly AcademiesDb.Repositories.PipelineEstablishmentRepository _sut;

    public GetAcademiesPipelineSummaryAsyncTests()
    {
        _sut = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);
    }

    [Fact]
    public async Task ForFreeSchool_ShouldCalculateCorrectNumberOfResults()
    {
        var trustReferences = "TRU123";

        _mockContext.AddMstrFreeSchoolProject(trustReferences, PipelineStatuses.FreeSchoolPipeline, "FreeRoute");

        var result = await _sut.GetAcademiesPipelineSummaryAsync(trustReferences);

        result.FreeSchoolsCount.Should().Be(1);
        result.PreAdvisoryCount.Should().Be(0);
        result.PostAdvisoryCount.Should().Be(0);
    }

    [Theory]
    [InlineData(null, null, 0)]
    [InlineData(null, true, 0)]
    [InlineData(true, null, 0)]
    [InlineData(true, false, 2)]
    public async Task ForPreAdvisoryConversion_ShouldCalculateCorrectCount(bool? inPrepare, bool? inComplete,
        int expectedCount)
    {
        var trustReference = "TRU123";

        _mockContext.AddMstrAcademyConversion(
            trustReference,
            PipelineStatuses.ApprovedForAO,
            routeOfProject: ProjectType.Conversion,
            inPrepare: inPrepare,
            inComplete: inComplete);

        _mockContext.AddMstrAcademyTransfer(
            trustReference,
            inPrepare: inPrepare,
            inComplete: inComplete,
            academyTransferStatus: PipelineStatuses.InProcessOfAcademyTransfer
        );

        var result = await _sut.GetAcademiesPipelineSummaryAsync(trustReference);

        result.PreAdvisoryCount.Should().Be(expectedCount);
    }


    [Theory]
    [InlineData(null, null, 0)]
    [InlineData(true, true, 2)]
    [InlineData(true, null, 0)]
    [InlineData(true, false, 0)]
    public async Task ForPostAdvisoryConversion_ShouldCalculateCorrectCount(bool? inPrepare, bool? inComplete,
        int expectedCount)
    {
        var trustReference = "TRU123";

        _mockContext.AddMstrAcademyConversion(
            trustReference,
            PipelineStatuses.ApprovedForAO,
            inPrepare,
            inComplete
        );

        _mockContext.AddMstrAcademyTransfer(
            trustReference,
            PipelineStatuses.InProcessOfAcademyTransfer,
            inPrepare,
            inComplete
        );

        var result = await _sut.GetAcademiesPipelineSummaryAsync(trustReference);

        result.PostAdvisoryCount.Should().Be(expectedCount);
    }

    [Fact]
    public async Task IfFreeSchoolExists_ShouldReurnCorrectNumberOfFreeSchools()
    {
        var freeSchoolTrust = "FREE";

        _mockContext.AddMstrFreeSchoolProject(
            freeSchoolTrust,
            PipelineStatuses.FreeSchoolPipeline,
            "FreeRoute");

        var result = await _sut.GetAcademiesPipelineSummaryAsync(freeSchoolTrust);

        result.FreeSchoolsCount.Should().Be(1);
    }

    [Fact]
    public async Task IfFreeSchoolPipelineIsNotFreeSchool_ShouldReturnCountOf0()
    {
        var freeSchoolTrust = "FREE";

        _mockContext.AddMstrFreeSchoolProject(
            freeSchoolTrust,
            PipelineStatuses.ApprovedForAO,
            "FreeRoute");

        var result = await _sut.GetAcademiesPipelineSummaryAsync(freeSchoolTrust);

        result.FreeSchoolsCount.Should().Be(0);
    }

    [Fact]
    public async Task IfTrustDoesNotHaveFreeSchools_ShouldReturnCountOf0()
    {
        var freeSchoolTrust = "FREE";

        _mockContext.AddMstrFreeSchoolProject(
            freeSchoolTrust,
            PipelineStatuses.FreeSchoolPipeline,
            "FreeRoute");

        var result = await _sut.GetAcademiesPipelineSummaryAsync("SomeOtherTrust");

        result.FreeSchoolsCount.Should().Be(0);
    }
}
