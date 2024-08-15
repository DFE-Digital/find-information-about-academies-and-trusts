using DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;

namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IFreeSchoolMealsAverageProvider
{
    public double GetLaAverage(Academy academy);
    public double GetNationalAverage(Academy academy);
    public DataSource GetFreeSchoolMealsUpdated();
}
