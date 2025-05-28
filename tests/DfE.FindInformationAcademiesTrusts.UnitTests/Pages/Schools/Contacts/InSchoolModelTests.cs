using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Contacts;

public class InSchoolModelTests : BaseContactsAreaModelTests<InSchoolModel>
{
    private readonly InSchoolContactsServiceModel _dummyInSchoolContacts = new(new ContactModel("Head teacher name", "head-teacher",
        new Person("Aaron Aaronson", "aa@someschool.com")));

    public InSchoolModelTests()
    {
        Sut = new InSchoolModel(MockSchoolService, MockTrustService, MockDataSourceService);
    }
    
    public override async Task OnGetAsync_should_configure_PageMetadata_SubPageName()
    {
        await OnGetAsync_should_configure_PageMetadata_SubPageName_for_school();
        await OnGetAsync_should_configure_PageMetadata_SubPageName_for_academy();
    }

    private async Task OnGetAsync_should_configure_PageMetadata_SubPageName_for_school()
    {
        Sut.Urn = SchoolUrn;

        await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Contacts in this school");
    }

    private async Task OnGetAsync_should_configure_PageMetadata_SubPageName_for_academy()
    {
        Sut.Urn = AcademyUrn;

        await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Contacts in this academy");
    }
}