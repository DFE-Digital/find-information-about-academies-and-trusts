using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class ApplicationSettingsFaker
{
    private readonly DateTime _refDate;

    public ApplicationSettingsFaker(DateTime refDate)
    {
        _refDate = refDate;
    }

    private ApplicationSetting CreateApplicationSetting(DateTime? modified, string key)
    {
        return new ApplicationSetting
        {
            Key = key,
            Modified = modified
        };
    }

    public IEnumerable<ApplicationSetting> Generate()
    {
        return new List<ApplicationSetting>
        {
            CreateApplicationSetting(_refDate.AddDays(-5), "ManagementInformationSchoolTableData CSV Filename"),
            CreateApplicationSetting(_refDate.AddDays(-8),
                "ManagementInformationFurtherEducationSchoolTableData CSV Filename")
        };
    }
}
