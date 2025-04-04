using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Ofsted;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export.Builders;

namespace DfE.FindInformationAcademiesTrusts.Services.Export;

public interface IExportService
{
    Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid);
    Task<byte[]> ExportOfstedDataToSpreadsheetAsync(string uid);
    Task<byte[]> ExportPipelineAcademiesToSpreadsheetAsync(string uid);
}

public class ExportService(
    IAcademyRepository academyRepository,
    IOfstedRepository ofstedRepository,
    ITrustRepository trustRepository,
    IAcademyService academyService) : IExportService
{

    public async Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid)
    {
        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var academiesDetails = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);
        var academiesOfstedRatings = await ofstedRepository.GetAcademiesInTrustOfstedAsync(uid);
        var academiesPupilNumbers = await academyRepository.GetAcademiesInTrustPupilNumbersAsync(uid);
        var academiesFreeSchoolMeals = await academyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        return new AcademiesBuilder()
            .WriteTrustInformation(trustSummary)
            .WriteHeaders()
            .WriteRows(academiesDetails, academiesOfstedRatings, academiesPupilNumbers, academiesFreeSchoolMeals)
            .Build();
    }

    public async Task<byte[]> ExportOfstedDataToSpreadsheetAsync(string uid)
    {
        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var academiesDetails = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);
        var academiesOfstedRatings = await ofstedRepository.GetAcademiesInTrustOfstedAsync(uid);

        return new OfstedDataBuilder()
            .WriteTrustInformation(trustSummary)
            .WriteHeaders()
            .WriteRows(academiesDetails, academiesOfstedRatings)
            .Build();
    }
    
    public async Task<byte[]> ExportPipelineAcademiesToSpreadsheetAsync(string uid)
    {
        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var trustReferenceNumber = await trustRepository.GetTrustReferenceNumberAsync(uid);
        var preAdvisoryAcademies = await academyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber);
        var postAdvisoryAcademies = await academyService.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber);
        var freeSchools = await academyService.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber);
        
        return new PipelineAcademiesBuilder()
            .WriteTrustInformation(trustSummary)
            .WriteHeadersForPreAdvisory()
            .WriteRows(preAdvisoryAcademies)
            .WriteHeadersForPostAdvisory()
            .WriteRows(postAdvisoryAcademies)
            .WriteHeadersForFreeSchools()
            .WriteRows(freeSchools)
            .Build();
    }
}
