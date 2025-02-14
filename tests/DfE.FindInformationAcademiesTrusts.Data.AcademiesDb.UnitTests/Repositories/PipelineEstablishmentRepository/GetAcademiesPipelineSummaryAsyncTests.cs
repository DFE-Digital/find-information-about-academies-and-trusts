using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using System.Collections.Generic;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories.PipelineEstablishmentRepository;

public class GetAcademiesPipelineSummaryAsyncTests
{
    private readonly MockAcademiesDbContext _mockContext;

    private readonly List<MstrAcademyConversions> _academyConversions;
    private readonly List<MstrAcademyTransfers> _academyTransfers;
    private readonly List<MstrFreeSchoolProject> _freeSchoolProjects;

    public GetAcademiesPipelineSummaryAsyncTests()
    {
        // Create mock context
        _mockContext = new MockAcademiesDbContext();

        _academyConversions = new List<MstrAcademyConversions>();
        _academyTransfers = new List<MstrAcademyTransfers>();
        _freeSchoolProjects = new List<MstrFreeSchoolProject>();

        // use mock context to setup above lists for testing purposes
        _mockContext.SetupMockDbContext(_academyConversions, db => db.MstrAcademyConversions);
        _mockContext.SetupMockDbContext(_academyTransfers, db => db.MstrAcademyTransfers);
        _mockContext.SetupMockDbContext(_freeSchoolProjects, db => db.MstrFreeSchoolProjects);
    }

    [Fact]
    public async Task ForFreeSchool_ShouldCalculateCorrectNumberOfResults()
    {
        string trustReferences = "TRU123";

        _freeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = 1,
            TrustID = trustReferences,
            Stage = PipelineStatuses.FreeSchoolPipeline,
            RouteOfProject = "FreeRoute"
        });

        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync(trustReferences);

        result.FreeSchoolsCount.Should().Be(1);
        result.PreAdvisoryCount.Should().Be(0);
        result.PostAdvisoryCount.Should().Be(0);
    }

    [Theory]
    [InlineData(null, null, 0)]
    [InlineData(null, true, 0)]
    [InlineData(true, null, 0)]
    [InlineData(true, false, 2)]
    public async Task ForPreAdvisoryConversion_ShouldCalculateCorrectCount(bool? inPrepare, bool? inComplete, int expectedCount)
    {
        string trustReference = "TRU123";

        _academyConversions.Add(new MstrAcademyConversions
        {
            SK = 2,
            TrustID = trustReference,
            ProjectStatus = PipelineStatuses.ApprovedForAO,
            RouteOfProject = ProjectType.Conversion,
            InPrepare = inPrepare,
            InComplete = inComplete,
        });

        _academyTransfers.Add(new MstrAcademyTransfers
        {
            SK = 3,
            InPrepare = inPrepare,
            InComplete = inComplete,
            NewProvisionalTrustID = trustReference,
            AcademyTransferStatus = PipelineStatuses.InProcessOfAcademyTransfer,
        });

        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync(trustReference);

        result.PreAdvisoryCount.Should().Be(expectedCount);
    }



    [Theory]
    [InlineData(null, null, 0)]
    [InlineData(true, true, 2)]
    [InlineData(true, null, 0)]
    [InlineData(true, false, 0)]
    public async Task ForPostAdvisoryConversion_ShouldCalculateCorrectCount(bool? inPrepare, bool? inComplete, int expectedCount)
    {
        string trustReference = "TRU123";

        _academyConversions.Add(new MstrAcademyConversions
        {
            SK = 2,
            TrustID = trustReference,
            ProjectStatus = PipelineStatuses.ApprovedForAO,
            RouteOfProject = ProjectType.Conversion,
            InPrepare = inPrepare,
            InComplete = inComplete,
        });

        _academyTransfers.Add(new MstrAcademyTransfers
        {
            SK = 3,
            InPrepare = inPrepare,
            InComplete = inComplete,
            NewProvisionalTrustID = trustReference,
            AcademyTransferStatus = PipelineStatuses.InProcessOfAcademyTransfer,
        });

        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync(trustReference);

        result.PostAdvisoryCount.Should().Be(expectedCount);
    }

    [Fact]
    public async Task IfFreeSchoolExists_ShouldReurnCorrectNumberOfFreeSchools()
    {
        string freeSchoolTrust = "FREE";

        _freeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = _freeSchoolProjects.Count + 1,
            TrustID = freeSchoolTrust,
            Stage = PipelineStatuses.FreeSchoolPipeline,
            RouteOfProject = "FreeRoute"
        });
            
        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync(freeSchoolTrust);

        result.FreeSchoolsCount.Should().Be(1);
    }

    [Fact]
    public async Task IfFreeSchoolPipelineIsNotFreeSchool_ShouldReturnCountOf0()
    {
        string freeSchoolTrust = "FREE";

        _freeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = _freeSchoolProjects.Count + 1,
            TrustID = freeSchoolTrust,
            Stage = PipelineStatuses.ApprovedForAO,
            RouteOfProject = "FreeRoute"
        });

        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync(freeSchoolTrust);

        result.FreeSchoolsCount.Should().Be(0);
    }

    [Fact]
    public async Task IfTrustDoesNotHaveFreeSchools_ShouldReturnCountOf0()
    {
        string freeSchoolTrust = "FREE";

        _freeSchoolProjects.Add(new MstrFreeSchoolProject
        {
            SK = _freeSchoolProjects.Count + 1,
            TrustID = freeSchoolTrust,
            Stage = PipelineStatuses.FreeSchoolPipeline,
            RouteOfProject = "FreeRoute"
        });

        var repository = new AcademiesDb.Repositories.PipelineEstablishmentRepository(_mockContext.Object);

        var result = await repository.GetAcademiesPipelineSummaryAsync("SomeOtherTrust");

        result.FreeSchoolsCount.Should().Be(0);
    }
}