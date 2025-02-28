using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories.PipelineEstablishmentRepository;

public class GetAcademiesPipelineSummaryAsyncTests
{
    private const string TrustReferenceNumber = "TRU123";
    private readonly MockAcademiesDbContext _mockContext = new();
    private readonly AcademiesDb.Repositories.PipelineEstablishmentRepository _sut;

    public GetAcademiesPipelineSummaryAsyncTests()
    {
        _sut = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);
    }

    [Theory]
    [InlineData(null, null, 0)]
    [InlineData(null, true, 0)]
    [InlineData(true, null, 0)]
    [InlineData(true, false, 2)]
    public async Task ForPreAdvisoryConversion_ShouldCalculateCorrectCount(bool? inPrepare, bool? inComplete,
        int expectedCount)
    {
        _mockContext.AddMstrAcademyConversion(
            TrustReferenceNumber,
            PipelineStatuses.ApprovedForAO,
            inPrepare,
            inComplete);

        _mockContext.AddMstrAcademyTransfer(
            TrustReferenceNumber,
            inPrepare: inPrepare,
            inComplete: inComplete,
            academyTransferStatus: PipelineStatuses.InProcessOfAcademyTransfer
        );

        var result = await _sut.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber);

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
        _mockContext.AddMstrAcademyConversion(
            TrustReferenceNumber,
            PipelineStatuses.ApprovedForAO,
            inPrepare,
            inComplete
        );

        _mockContext.AddMstrAcademyTransfer(
            TrustReferenceNumber,
            PipelineStatuses.InProcessOfAcademyTransfer,
            inPrepare,
            inComplete
        );

        var result = await _sut.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber);

        result.PostAdvisoryCount.Should().Be(expectedCount);
    }

    [Fact]
    public async Task ForConversions_ShouldNotIncludeDaoRevoked()
    {
        //Pre-advisory
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PreAdvisory,
            PipelineStatuses.ConverterPreAO, "Pre-Academy convertor 1");
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PreAdvisory,
            PipelineStatuses.DirectiveAcademyOrders, "Pre-Academy revoked", "dAO revoked");
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PreAdvisory,
            PipelineStatuses.DirectiveAcademyOrders, "Pre-Academy convertor 2",
            "Sponsor funding confirmed – progressing to conversion");

        //Post-advisory
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PostAdvisory,
            PipelineStatuses.ApprovedForAO, "Post-Academy convertor 1");
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PostAdvisory,
            PipelineStatuses.DirectiveAcademyOrders, "Post-Academy revoked", "dAO revoked");
        _mockContext.AddMstrAcademyConversion(TrustReferenceNumber, AdvisoryType.PostAdvisory,
            PipelineStatuses.DirectiveAcademyOrders, "Post-Academy convertor 2",
            "Sponsor funding confirmed – progressing to conversion");

        var result = await _sut.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber);

        result.PreAdvisoryCount.Should().Be(2);
        result.PostAdvisoryCount.Should().Be(2);
    }

    [Fact]
    public async Task IfTrustDoesNotHaveFreeSchools_ShouldReturnCountOf0()
    {
        var result = await _sut.GetAcademiesPipelineSummaryAsync("SomeOtherTrust");

        result.FreeSchoolsCount.Should().Be(0);
    }

    [Fact]
    public async Task ForFreeSchoolsCount_ShouldOnlyReturnCountOfFreeSchoolsInPipeline()
    {
        _mockContext.AddMstrFreeSchoolProject(TrustReferenceNumber, projectName: "Pipeline School");
        _mockContext.AddMstrFreeSchoolProject(TrustReferenceNumber, projectName: "Already open school", stage: "Open");
        _mockContext.AddMstrFreeSchoolProject(TrustReferenceNumber, projectName: "Another school in pipeline");

        var result = await _sut.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber);

        result.FreeSchoolsCount.Should().Be(2);
    }
}
