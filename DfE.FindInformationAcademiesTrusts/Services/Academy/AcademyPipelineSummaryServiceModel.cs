namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademyPipelineSummaryServiceModel(int PreAdvisoryCount, int PostAdvisoryCount, int FreeSchoolsCount)
{
    public int Total => PreAdvisoryCount + PostAdvisoryCount + FreeSchoolsCount;
}