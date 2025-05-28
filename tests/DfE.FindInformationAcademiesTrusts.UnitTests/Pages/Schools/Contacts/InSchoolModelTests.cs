using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.Contacts;

public class InSchoolModelTests : BaseContactsAreaModelTests<InSchoolModel>
{
    private readonly ISchoolContactsService _mockSchoolContactsService = Substitute.For<ISchoolContactsService>();

    private readonly ContactModel _dummyInSchoolContacts = new ("Head teacher name", "head-teacher",
        new Person("Aaron Aaronson", "aa@someschool.com"));

    public InSchoolModelTests()
    {
        _mockSchoolContactsService.GetInSchoolContactsAsync(Arg.Any<int>()).Returns(_dummyInSchoolContacts);

        Sut = new InSchoolModel(MockSchoolService, MockTrustService, _mockSchoolContactsService, MockDataSourceService)
            { Urn = SchoolUrn };
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
    
    [Theory]
    [InlineData(SchoolCategory.LaMaintainedSchool, "Contacts in this school")]
    [InlineData(SchoolCategory.Academy, "Contacts in this academy")]
    public void SubPageName_should_include_school_type(SchoolCategory schoolCategory, string expectedSubPageName)
    {
        InSchoolModel.SubPageName(schoolCategory).Should().Be(expectedSubPageName);
    }

    [Fact]
    public void SubPageName_should_throw_for_unrecognised_school_type()
    {
        var act = () => InSchoolModel.SubPageName((SchoolCategory)999);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public async Task OnGetAsync_should_set_Headteacher_contact_details()
    {
        await Sut.OnGetAsync();

        Sut.HeadTeacher.Should().Be(_dummyInSchoolContacts);
    }
}