using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy;

namespace PipelineEstablishmentRepositoryTests
{

    public class PipelineEstablishmentRepositoryTests
    {
        private readonly MockAcademiesDbContext _mockContext;

        private readonly List<MstrAcademyConversions> _academyConversions;
        private readonly List<MstrAcademyTransfers> _academyTransfers;
        private readonly List<MstrFreeSchoolProject> _freeSchoolProjects;

        public PipelineEstablishmentRepositoryTests()
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

        #region Helper Methods for Seeding tests

        private void AddMstrFreeSchoolProject(
            string trustId,
            string stage,
            string routeOfProject,
            string? projectName = null,
            int? statutoryLowestAge = null,
            int? statutoryHighestAge = null,
            int? newUrn = null,
            string? localAuthority = null,
            DateTime? actualDateOpened = null)
        {
            _freeSchoolProjects.Add(new MstrFreeSchoolProject
            {
                SK = _freeSchoolProjects.Count + 1,
                TrustID = trustId,
                Stage = stage,
                RouteOfProject = routeOfProject,
                ProjectName = projectName,
                StatutoryLowestAge = statutoryLowestAge,
                StatutoryHighestAge = statutoryHighestAge,
                NewURN = newUrn,
                LocalAuthority = localAuthority,
                ActualDateOpened = actualDateOpened
            });
        }

        private void AddMstrAcademyConversion(
            string trustId,
            string projectStatus,
            string inPrepare,
            bool inComplete,
            string routeOfProject = "Conversion",
            string? projectName = null,
            int? urn = null,
            int? statutoryLowestAge = null,
            int? statutoryHighestAge = null,
            DateTime? expectedOpeningDate = null)
        {
            _academyConversions.Add(new MstrAcademyConversions
            {
                SK = _academyConversions.Count + 1,
                TrustID = trustId,
                ProjectStatus = projectStatus,
                InPrepare = inPrepare,
                InComplete = inComplete,
                RouteOfProject = routeOfProject,
                ProjectName = projectName,
                URN = urn,
                StatutoryLowestAge = statutoryLowestAge,
                StatutoryHighestAge = statutoryHighestAge,
                ExpectedOpeningDate = expectedOpeningDate
            });
        }

        private void AddMstrAcademyTransfer(
            string trustId,
            string academyTransferStatus,
            string inPrepare,
            bool inComplete,
            string? academyName = null,
            int? academyUrn = null,
            int? statutoryLowestAge = null,
            int? statutoryHighestAge = null,
            string? localAuthority = null,
            DateTime? expectedTransferDate = null)
        {
            _academyTransfers.Add(new MstrAcademyTransfers
            {
                SK = _academyTransfers.Count + 1,
                NewProvisionalTrustID = trustId,
                AcademyTransferStatus = academyTransferStatus,
                InPrepare = inPrepare,
                InComplete = inComplete,
                AcademyName = academyName,
                AcademyURN = academyUrn,
                StatutoryLowestAge = statutoryLowestAge,
                StatutoryHighestAge = statutoryHighestAge,
                LocalAuthority = localAuthority,
                ExpectedTransferDate = expectedTransferDate
            });
        }

        #endregion

        [Fact]
        public async Task GetPipelineFreeSchoolProjectsAsync_HappyPath_ReturnsCorrectResults()
        {
            // Arrange
            var trustRef = "TR100";
            AddMstrFreeSchoolProject(trustRef, PipelineStatuses.FreeSchoolPipeline, "FreeRoute",
                                     projectName: "Project A", statutoryLowestAge: 5, statutoryHighestAge: 11,
                                     newUrn: 12345, localAuthority: "LA A", actualDateOpened: new DateTime(2024, 9, 1));

            // Add a non-matching project (different stage)
            AddMstrFreeSchoolProject(trustRef, "NotPipeline", "FreeRoute");

            // Add a project for a different trust
            AddMstrFreeSchoolProject("TR999", PipelineStatuses.FreeSchoolPipeline, "FreeRoute");

            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetPipelineFreeSchoolProjectsAsync(trustRef);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1, "because only one project matches stage 'Pipeline' for TR100");

            var project = result![0];
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
            // Arrange
            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetPipelineFreeSchoolProjectsAsync("TR_NOT_EXIST");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty("because no matching free school projects were added for this trust");
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
            AddMstrAcademyConversion(trustRef, validStatuses[0], inPrepare: "Yes", inComplete: false,
                                     projectName: "Conversion X", urn: 555, statutoryLowestAge: 3, statutoryHighestAge: 7,
                                     expectedOpeningDate: new DateTime(2025, 4, 1));

            // Add some that won't match
            // 1) Wrong trust
            AddMstrAcademyConversion("TR999", validStatuses[0], "Yes", false);
            // 2) Wrong status
            AddMstrAcademyConversion(trustRef, "SomeOtherStatus", "Yes", false);
            // 3) InComplete = "No", InPrepare = "No" => fails the pre-advisory filter
            AddMstrAcademyConversion(trustRef, validStatuses[1], "No", false);

            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryConversionEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1, "only one record matches the trust, valid status, and pre-advisory flags");

            var est = result![0];
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
            AddMstrAcademyConversion(trustRef, validStatuses[2], inPrepare: "No", true,
                                     projectName: "PostAdvisory Conv", urn: 999);

            // Non-matching: (InComplete="No")
            AddMstrAcademyConversion(trustRef, validStatuses[3], inPrepare: "No", true);

            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryConversionEstablishmentsAsync(trustRef, AdvisoryType.PostAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result![0].Urn.Should().Be("999");
            result[0].EstablishmentName.Should().Be("PostAdvisory Conv");
        }

        [Fact]
        public async Task GetAdvisoryConversionEstablishmentsAsync_NoData_ReturnsEmptyArray()
        {
            // Arrange
            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryConversionEstablishmentsAsync("TR_NO_MATCH", AdvisoryType.PreAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
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
            AddMstrAcademyTransfer(trustRef, validStatuses[0], inPrepare: "Yes", false,
                                   academyName: "Transfer Academy 1", academyUrn: 111,
                                   statutoryLowestAge: 5, statutoryHighestAge: 10,
                                   localAuthority: "LA B", expectedTransferDate: new DateTime(2026, 1, 1));

            // Add some that won't match
            // 1) Different trust
            AddMstrAcademyTransfer("TR999", validStatuses[0], "Yes", false, academyName: "Wrong Trust");
            // 2) Wrong status
            AddMstrAcademyTransfer(trustRef, "RandomStatus", "Yes", false, academyName: "Wrong Status");
            // 3) InComplete="Yes"
            AddMstrAcademyTransfer(trustRef, validStatuses[1], "Yes", true, academyName: "Wrong InComplete");

            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PreAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1, "only one transfer matches trust, valid status, and pre-advisory flags");

            var est = result![0];
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
            AddMstrAcademyTransfer(trustRef, validStatuses[1], inPrepare: "No", true,
                                   academyName: "Post Transfer Academy", academyUrn: 777);

            // Add a non-matching record: same trust, same status but InComplete="No"
            AddMstrAcademyTransfer(trustRef, validStatuses[1], inPrepare: "No", false,
                                   academyName: "Non-post Transfer Academy", academyUrn: 778);

            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryTransferEstablishmentsAsync(trustRef, AdvisoryType.PostAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result![0].Urn.Should().Be("777");
            result[0].EstablishmentName.Should().Be("Post Transfer Academy");
        }

        [Fact]
        public async Task GetAdvisoryTransferEstablishmentsAsync_NoData_ReturnsEmptyArray()
        {
            // Arrange
            var repository = new PipelineEstablishmentRepository(_mockContext.Object);

            // Act
            var result = await repository.GetAdvisoryTransferEstablishmentsAsync("TR_NO_DATA", AdvisoryType.PreAdvisory);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
