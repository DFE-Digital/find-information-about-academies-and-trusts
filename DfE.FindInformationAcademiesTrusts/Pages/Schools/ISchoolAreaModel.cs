using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools;

public interface ISchoolAreaModel
{
    int Urn { get; }
    SchoolCategory SchoolCategory { get; }
    List<DataSourcePageListEntry> DataSourcesPerPage { get; }
    PageMetadata PageMetadata { get; }
    string SchoolName { get; }
    string SchoolType { get; }
}
