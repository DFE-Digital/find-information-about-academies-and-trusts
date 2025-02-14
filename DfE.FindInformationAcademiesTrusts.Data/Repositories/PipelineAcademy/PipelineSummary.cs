namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy
{
    public record PipelineSummary(
    int PreAdvisoryCount,
    int PostAdvisoryCount,
    int FreeSchoolsCount
)
    {
        public static int CalculateTotal(int preAdvisory, int postAdvisory, int freeSchools)
        => preAdvisory + postAdvisory + freeSchools;
    }


}
