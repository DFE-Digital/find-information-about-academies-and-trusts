using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.SchoolNavMenu;

public abstract class SchoolNavMenuTestsBase
{
    public static TheoryData<Type> SubPageTypes =>
    [
        //Overview
        typeof(DetailsModel)
    ];

    protected static SchoolAreaModel GetMockSchoolPage(Type pageType, string? urn = null,
        SchoolCategory schoolCategory = SchoolCategory.LaMaintainedSchool)
    {
        //Create a mock page
        var parameters = pageType.GetConstructors()[0].GetParameters();
        var arguments = parameters.Select(p => p.ParameterType.Name switch
        {
            _ => Substitute.For([p.ParameterType], [])
        }).ToArray();

        var mockPage = Activator.CreateInstance(pageType, arguments) as SchoolAreaModel ??
                       throw new ArgumentException("Couldn't create mock for given page type", nameof(pageType));

        //Set properties applicable to all types
        mockPage.Urn = urn ?? (schoolCategory == SchoolCategory.Academy ? "123456" : "222222");

        return mockPage;
    }
}
