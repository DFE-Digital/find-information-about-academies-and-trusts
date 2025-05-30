using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SchoolContactsServiceTests
{
    private readonly int _urn = 123;
    private readonly SchoolContactsService _sut;
    private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();

    private readonly SchoolContact _dummySchoolContact = new("Teacher", "McTeacherson", "a.teacher@school.com");

    public SchoolContactsServiceTests()
    {
        _sut = new SchoolContactsService(_mockSchoolRepository);
    }

    [Fact]
    public async Task should_set_contact_details_correctly()
    {
        var expectedResult = new ContactModel("Head teacher", "head-teacher",
            new Person("Teacher McTeacherson", "a.teacher@school.com"));
        
        _mockSchoolRepository.GetSchoolContactsAsync(_urn).Returns(_dummySchoolContact);
        
        var result = await _sut.GetInSchoolContactsAsync(_urn);
        
        result.Should().BeEquivalentTo(expectedResult);
    }
}