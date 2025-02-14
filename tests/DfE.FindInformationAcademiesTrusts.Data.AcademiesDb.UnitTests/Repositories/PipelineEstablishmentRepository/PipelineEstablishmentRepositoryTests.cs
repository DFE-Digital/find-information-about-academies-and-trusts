using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories.PipelineEstablishmentRepository;

public class PipelineEstablishmentRepositoryTests
{
    private readonly MockAcademiesDbContext _mockContext = new();
    private readonly AcademiesDb.Repositories.PipelineEstablishmentRepository _sut;

    public PipelineEstablishmentRepositoryTests()
    {
        _sut = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetPipelineFreeSchoolProjectsAsync_HappyPath_ReturnsCorrectResults()
    {
        // Arrange
        var trustRef = "TR100";
        _mockContext.AddMstrFreeSchoolProject(trustRef, PipelineStatuses.FreeSchoolPipeline, "FreeRoute",
            "Project A", 5, 11,
            12345, "LA A", new DateTime(2024, 9, 1));

        // Add a non-matching project (different stage)
        _mockContext.AddMstrFreeSchoolProject(trustRef, "NotPipeline", "FreeRoute");

        // Add a project for a different trust
        _mockContext.AddMstrFreeSchoolProject("TR999", PipelineStatuses.FreeSchoolPipeline, "FreeRoute");

        // Act
        var result = await _sut.GetPipelineFreeSchoolProjectsAsync(trustRef);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1, "because only one project matches stage 'Pipeline' for TR100");

        var project = result[0];
        project.Urn.Should().Be("12345");
        project.EstablishmentName.Should().Be("Project A");
        project.ProjectType.Should().Be("FreeRoute");
        project.LocalAuthority.Should().Be("LA A");
        project.AgeRange.Should().NotBeNull();
        project.AgeRange!.Minimum.Should().Be(5);
        project.AgeRange.Maximum.Should().Be(11);
        project.ChangeDate.Should().Be(new DateTime(2024, 9, 1));
    }

    [Fact]
    public async Task GetPipelineFreeSchoolProjectsAsync_NoData_ReturnsEmptyArray()
    {
        // Act
        var result = await _sut.GetPipelineFreeSchoolProjectsAsync("TR_NOT_EXIST");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty("because no matching free school projects were added for this trust");
    }


    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(1, null, null, null)]
    [InlineData(null, 1, null, null)]
    [InlineData(1, 5, 1, 5)]
    public async Task GetPipelineFreeSchoolProjectsAsync_WithNoLowAndHighAgeRange_ShouldHaveNullAgeRange(int? lowerAge,
        int? upperAge, int? expectedMin, int? expectedMax)
    {
        var trustRef = "TR_NO_AGES";

        _mockContext.AddMstrFreeSchoolProject(trustRef, PipelineStatuses.FreeSchoolPipeline, "FreeRoute",
            "Project A", lowerAge, upperAge,
            12345, "LA A", new DateTime(2024, 9, 1));

        // Act
        var result = await _sut.GetPipelineFreeSchoolProjectsAsync(trustRef);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var project = result[0];
        if (expectedMin.HasValue && expectedMax.HasValue)
        {
            project.AgeRange.Should().NotBeNull();
            project.AgeRange!.Minimum.Should().Be(expectedMin.Value);
            project.AgeRange.Maximum.Should().Be(expectedMax.Value);
        }
        else
        {
            project.AgeRange.Should().BeNull();
        }
    }

    [Fact]
    public async Task GetAdvisoryConversionEstablishmentsAsync_PreAdvisory_ReturnsCorrectResults()
    {
        // Arrange
        var trustRef = "TR200";

        // Valid statuses
        var validStatuses = new[]
        {
            PipelineStatuses.ApprovedForAO,
            PipelineStatuses.AwaitingModeration,
            PipelineStatuses.ConverterPreAO,
            PipelineStatuses.ConverterPreAOC,
            PipelineStatuses.Deferred,
            PipelineStatuses.DirectiveAcademyOrders
        };

        // Add a matching record: InComplete = "No", InPrepare = "Yes"
        _mockContext.AddMstrAcademyConversion(trustRef, validStatuses[0], true, false,
            projectName: "Conversion X", urn: 555, statutoryLowestAge: 3, statutoryHighestAge: 7,
            expectedOpeningDate: new DateTime(2025, 4, 1));

        // Add some that won't match
        // 1) Wrong trust
        _mockContext.AddMstrAcademyConversion("TR999", validStatuses[0], true, false);
        // 2) Wrong status
        _mockContext.AddMstrAcademyConversion(trustRef, "SomeOtherStatus", true, false);
        // 3) InComplete = "No", InPrepare = "No" => fails the pre-advisory filter
        _mockContext.AddMstrAcademyConversion(trustRef, validStatuses[1], false, false);

        // Act
        var result = await _sut.GetAdvisoryConversionEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1, "only one record matches the trust, valid status, and pre-advisory flags");

        var est = result[0];
        est.Urn.Should().Be("555");
        est.EstablishmentName.Should().Be("Conversion X");
        est.ProjectType.Should().Be("Conversion");
        est.AgeRange.Should().NotBeNull();
        est.AgeRange!.Minimum.Should().Be(3);
        est.AgeRange.Maximum.Should().Be(7);
        est.ChangeDate.Should().Be(new DateTime(2025, 4, 1));
    }

    [Fact]
    public async Task GetAdvisoryConversionEstablishmentsAsync_PostAdvisory_ReturnsCorrectResults()
    {
        // Arrange
        var trustRef = "TR200";
        // Use the same valid statuses
        var validStatuses = new[]
        {
            PipelineStatuses.ApprovedForAO,
            PipelineStatuses.AwaitingModeration,
            PipelineStatuses.ConverterPreAO,
            PipelineStatuses.ConverterPreAOC,
            PipelineStatuses.Deferred,
            PipelineStatuses.DirectiveAcademyOrders
        };

        // Add a matching record: InComplete="Yes", InPrepare=anything (ignored in post-advisory)
        _mockContext.AddMstrAcademyConversion(trustRef, validStatuses[2], false, true,
            projectName: "PostAdvisory Conv", urn: 999);

        // Non-matching: (InComplete="No")
        _mockContext.AddMstrAcademyConversion(trustRef, validStatuses[3], false, false);

        // Act
        var result = await _sut.GetAdvisoryConversionEstablishmentsAsync(trustRef, AdvisoryType.PostAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Urn.Should().Be("999");
        result[0].EstablishmentName.Should().Be("PostAdvisory Conv");
    }

    [Fact]
    public async Task GetAdvisoryConversionEstablishmentsAsync_NoData_ReturnsEmptyArray()
    {
        // Arrange
        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        // Act
        var result = await repository.GetAdvisoryConversionEstablishmentsAsync("TR_NO_MATCH", AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(1, null, null, null)]
    [InlineData(null, 1, null, null)]
    [InlineData(1, 5, 1, 5)]
    public async Task GetAdvisoryConversionEstablishmentsAsync_WithNoLowAndHighAgeRange_ShouldHaveNullAgeRange(
        int? lowerAge, int? upperAge, int? expectedMin, int? expectedMax)
    {
        // Arrange
        var trustRef = "NO_AGES";

        _mockContext.AddMstrAcademyConversion(trustRef, PipelineStatuses.ApprovedForAO, true, false,
            projectName: "Conversion X", urn: 555, statutoryLowestAge: lowerAge, statutoryHighestAge: upperAge,
            expectedOpeningDate: new DateTime(2025, 4, 1));

        // Act
        var result = await _sut.GetAdvisoryConversionEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var project = result[0];
        if (expectedMin.HasValue && expectedMax.HasValue)
        {
            project.AgeRange.Should().NotBeNull();
            project.AgeRange!.Minimum.Should().Be(expectedMin.Value);
            project.AgeRange.Maximum.Should().Be(expectedMax.Value);
        }
        else
        {
            project.AgeRange.Should().BeNull();
        }
    }

    [Fact]
    public async Task GetAdvisoryTransferEstablishmentsAsync_PreAdvisory_ReturnsCorrectResults()
    {
        // Arrange
        var trustRef = "TR300";
        // statuses considered
        var validStatuses = new[]
        {
            PipelineStatuses.ConsideringAcademyTransfer,
            PipelineStatuses.InProcessOfAcademyTransfer
        };

        // Add a valid record: InComplete="No", InPrepare="Yes"
        _mockContext.AddMstrAcademyTransfer(trustRef, validStatuses[0], true, false,
            "Transfer Academy 1", 111,
            5, 10,
            "LA B", new DateTime(2026, 1, 1));

        // Add some that won't match
        // 1) Different trust
        _mockContext.AddMstrAcademyTransfer("TR999", validStatuses[0], true, false, "Wrong Trust");
        // 2) Wrong status
        _mockContext.AddMstrAcademyTransfer(trustRef, "RandomStatus", true, false, "Wrong Status");
        // 3) InComplete="Yes"
        _mockContext.AddMstrAcademyTransfer(trustRef, validStatuses[1], true, true, "Wrong InComplete");

        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1, "only one transfer matches trust, valid status, and pre-advisory flags");

        var est = result[0];
        est.Urn.Should().Be("111");
        est.EstablishmentName.Should().Be("Transfer Academy 1");
        est.LocalAuthority.Should().Be("LA B");
        est.ProjectType.Should().Be("Transfer");
        est.AgeRange.Should().NotBeNull();
        est.AgeRange!.Minimum.Should().Be(5);
        est.AgeRange.Maximum.Should().Be(10);
        est.ChangeDate.Should().Be(new DateTime(2026, 1, 1));
    }

    [Fact]
    public async Task GetAdvisoryTransferEstablishmentsAsync_PostAdvisory_ReturnsCorrectResults()
    {
        // Arrange
        var trustRef = "TR300";
        var validStatuses = new[]
        {
            PipelineStatuses.ConsideringAcademyTransfer,
            PipelineStatuses.InProcessOfAcademyTransfer
        };

        // Add a matching record for PostAdvisory: InComplete="Yes"
        _mockContext.AddMstrAcademyTransfer(trustRef, validStatuses[1], false, true,
            "Post Transfer Academy", 777);

        // Add a non-matching record: same trust, same status but InComplete="No"
        _mockContext.AddMstrAcademyTransfer(trustRef, validStatuses[1], false, false,
            "Non-post Transfer Academy", 778);

        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PostAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Urn.Should().Be("777");
        result[0].EstablishmentName.Should().Be("Post Transfer Academy");
    }

    [Fact]
    public async Task GetAdvisoryTransferEstablishmentsAsync_NoData_ReturnsEmptyArray()
    {
        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync("TR_NO_DATA", AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task IfInCompleteForInProcessIsNull_ReturnsEmptyArray()
    {
        var trustRef = "TR888Null";

        _mockContext.AddMstrAcademyTransfer(trustRef, PipelineStatuses.InProcessOfAcademyTransfer, true, null,
            "Post Transfer Academy", 777);

        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PostAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task IfInPrepareForInProcessIsNull_ReturnsEmptyArray()
    {
        var trustRef = "TRPrepNull";

        _mockContext.AddMstrAcademyTransfer(trustRef, PipelineStatuses.InProcessOfAcademyTransfer, null, true,
            "Post Transfer Academy", 777);

        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(1, null, null, null)]
    [InlineData(null, 1, null, null)]
    [InlineData(1, 5, 1, 5)]
    public async Task GetAdvisoryTransferEstablishmentsAsync_WithNoLowAndHighAgeRange_ShouldHaveNullAgeRange(
        int? lowerAge, int? upperAge, int? expectedMin, int? expectedMax)
    {
        // Arrange
        var trustRef = "TR_NOAGE";

        _mockContext.AddMstrAcademyTransfer(trustRef, PipelineStatuses.ConsideringAcademyTransfer, true, false,
            "Transfer Academy 1", 111,
            lowerAge, upperAge,
            "LA B", new DateTime(2026, 1, 1));

        // Act
        var result = await _sut.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(1);

        var project = result[0];
        if (expectedMin.HasValue && expectedMax.HasValue)
        {
            project.AgeRange.Should().NotBeNull();
            project.AgeRange!.Minimum.Should().Be(expectedMin.Value);
            project.AgeRange.Maximum.Should().Be(expectedMax.Value);
        }
        else
        {
            project.AgeRange.Should().BeNull();
        }
    }
}
