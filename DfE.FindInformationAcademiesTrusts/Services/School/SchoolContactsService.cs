using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.Contact;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolContactsService
{
    Task<ContactModel> GetInSchoolContactsAsync(int urn);
}

public class SchoolContactsService(ISchoolRepository schoolRepository) : ISchoolContactsService
{
    public async Task<ContactModel> GetInSchoolContactsAsync(int urn)
    {
        var schoolContacts = await schoolRepository.GetSchoolContactsAsync(urn);
        var headteacherFullname = $"{schoolContacts.HeadteacherFirstName} {schoolContacts.HeadteacherLastName}";

        var headteacher = new ContactModel("Head teacher", "head-teacher",
            new Person(headteacherFullname, schoolContacts.HeadteacherEmail));

        return headteacher;
    }
}