using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class SharepointTrustDocLinkQueryFilterTests
{
    [Fact]
    public void SharepointTrustDocLinkQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validSharepointTrustDocLink = new SharepointTrustDocLink
        {
            FolderPrefix = "AFS",
            TrustRefNumber = "TR00123456",
            DocumentFilename = "Trust Document",
            DocumentLink = "www.trustDocumentLink.com",
            FolderYear = 2019
        };

        var filterFunction = AcademiesDbContext.SharepointTrustDocLinkQueryFilter.Compile();

        SharepointTrustDocLink[] data =
        [
            validSharepointTrustDocLink,
            new()
            {
                FolderPrefix = "AFS",
                TrustRefNumber = "TR00123456",
                DocumentFilename = "Trust Document",
                DocumentLink = null,
                FolderYear = 2019
            },
            new()
            {
                FolderPrefix = "AFS",
                TrustRefNumber = null,
                DocumentFilename = "Trust Document",
                DocumentLink = "www.trustDocumentLink.com",
                FolderYear = 2019
            }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validSharepointTrustDocLink);
    }
}
