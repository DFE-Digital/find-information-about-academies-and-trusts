using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools;

[ExcludeFromCodeCoverage] //Temporary exclusion while this contains dummy data
public class SchoolAreaModel : BasePageModel, ISchoolAreaModel
{
    [BindProperty(SupportsGet = true)] public string Urn { get; set; } = "";

    public SchoolCategory SchoolCategory => Urn.EndsWith('2')
        ? SchoolCategory.LaMaintainedSchool
        : SchoolCategory.Academy;

    public List<DataSourcePageListEntry> DataSourcesPerPage { get; set; } = [];
    public virtual PageMetadata PageMetadata => new(SchoolName, ModelState.IsValid);

    public string SchoolName
        => Urn.EndsWith('2')
            ? $"Cool School {Urn}"
            : $"Chill Academy {Urn}";

    public string SchoolType
        => Urn.EndsWith('2')
            ? "Community school"
            : "Academy sponsor led";
}
