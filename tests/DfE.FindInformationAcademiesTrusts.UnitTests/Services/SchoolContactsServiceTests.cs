using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Contacts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Services.School;
using NSubstitute.ReturnsExtensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class SchoolContactsServiceTests
{
    private readonly int _urn = 123;
    private readonly SchoolContactsService _sut;
    private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();
    private readonly IContactRepository _mockContactsRepository = Substitute.For<IContactRepository>();

    private readonly SchoolContact _dummySchoolContact = new("Teacher McTeacherson", "a.teacher@school.com");

    private readonly SchoolInternalContacts _dummyInternalContacts =
        new(new InternalContact("Regions Group Local Authority Lead",
            "regions.group.local.authority.lead@education.gov.uk", DateTime.Now, "some.user@education.gov.uk"));

    public SchoolContactsServiceTests()
    {
        _sut = new SchoolContactsService(_mockSchoolRepository, _mockContactsRepository);
    }

    [Fact]
    public async Task GetInSchoolContactsAsync_should_set_contact_details_from_repository()
    {
        var expectedResult = new Person("Teacher McTeacherson", "a.teacher@school.com");

        _mockSchoolRepository.GetSchoolContactsAsync(_urn).Returns(_dummySchoolContact);

        var result = await _sut.GetInSchoolContactsAsync(_urn);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetInSchoolContactsAsync_should_default_null_name_to_empty_string()
    {
        var expectedResult = new Person(string.Empty, "a.teacher@school.com");

        _mockSchoolRepository.GetSchoolContactsAsync(_urn).Returns(_dummySchoolContact with { Name = null });

        var result = await _sut.GetInSchoolContactsAsync(_urn);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetInSchoolContactsAsync_ShouldReturnNull_IfDataIsNullFromRepository()
    {
        _mockSchoolRepository.GetSchoolContactsAsync(_urn).ReturnsNull();

        var result = await _sut.GetInSchoolContactsAsync(_urn);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetInternalContactsAsync_should_set_RegionsGroupLocalAuthorityLead_details_from_repository()
    {
        var expectedResult = new SchoolInternalContactsServiceModel(new Person("Regions Group Local Authority Lead",
            "regions.group.local.authority.lead@education.gov.uk"));

        _mockContactsRepository.GetSchoolInternalContactsAsync(_urn).Returns(_dummyInternalContacts);

        var result = await _sut.GetInternalContactsAsync(_urn);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetInternalContactsAsync_should_default_null_RegionsGroupLocalAuthorityLead_to_empty_string_name_and_null_email()
    {
        var expectedResult = new SchoolInternalContactsServiceModel(new Person(string.Empty, null));

        _mockContactsRepository.GetSchoolInternalContactsAsync(_urn).Returns(new SchoolInternalContacts(null));

        var result = await _sut.GetInternalContactsAsync(_urn);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
