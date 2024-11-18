using DfE.FIAT.Data.Repositories.DataSource;

namespace DfE.FIAT.Data;

public interface IFreeSchoolMealsAverageProvider
{
    public double GetLaAverage(int localAuthorityCode, string? phaseOfEducation, string? typeOfEstablishment);
    public double GetNationalAverage(string? phaseOfEducation, string? typeOfEstablishment);
    public DataSource GetFreeSchoolMealsUpdated();
}
