using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolContactsService
{
    Task<Person?> GetInSchoolContactsAsync(int urn);
}

public class SchoolContactsService(ISchoolRepository schoolRepository) : ISchoolContactsService
{
    public async Task<Person?> GetInSchoolContactsAsync(int urn)
    {
        var schoolContacts = await schoolRepository.GetSchoolContactsAsync(urn);

        if (schoolContacts is null)
        {
            return null;
        }

        var headteacher = new Person(schoolContacts.Name ?? string.Empty, schoolContacts.Email);

        return headteacher;
    }
}
