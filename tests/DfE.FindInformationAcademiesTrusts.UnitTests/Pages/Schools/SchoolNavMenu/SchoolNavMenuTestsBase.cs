using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.SchoolNavMenu;

public abstract class SchoolNavMenuTestsBase
{
    public static TheoryData<Type> SubPageTypes =>
    [
        //Overview
        typeof(DetailsModel),
        typeof(SenModel),
        typeof(FederationModel)
    ];

    protected static SchoolAreaModel GetMockSchoolPage(Type pageType, int urn = 123456,
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
        mockPage.Urn = urn;
        mockPage.SchoolSummary = new SchoolSummaryServiceModel(urn, "Chill primary school", "", schoolCategory);

        return mockPage;
    }
}
