using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;

public class InSchoolModel(ISchoolService schoolService, ITrustService trustService)
    : ContactsAreaModel(schoolService, trustService)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName(SchoolCategory)
    };

    public static string SubPageName(SchoolCategory schoolCategory)
    {
        return schoolCategory switch
        {
            SchoolCategory.LaMaintainedSchool => "Contacts in this school",
            SchoolCategory.Academy => "Contacts in this academy",
            _ => throw new ArgumentOutOfRangeException(nameof(schoolCategory))
        };
    }
}
