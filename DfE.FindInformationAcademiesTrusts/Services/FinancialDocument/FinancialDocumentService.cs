using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

namespace DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

public interface IFinancialDocumentService
{
    Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(string uid,
        FinancialDocumentType financialDocumentType);
}

public class FinancialDocumentService(
    ITrustDocumentRepository trustDocumentRepository,
    IDateTimeProvider dateTimeProvider) : IFinancialDocumentService
{
    public enum FinancialYearSubmissionWindowStatus
    {
        Open,
        Closed
    }

    public (FinancialYear FinancialYear, FinancialYearSubmissionWindowStatus SubmissionWindowStatus)[]
        GetFinancialYearsToDisplay(FinancialDocumentType financialDocumentType)
    {
        // A financial year runs from 1st September to 31st August
        // The submission window for financial documents starts after that year has ended (and varies by financial document type)
        // We display the latest financial year with an open submission window and the two years before that

        var today = dateTimeProvider.Today;

        int latestFinancialYearToDisplay;

        if (financialDocumentType == FinancialDocumentType.SelfAssessmentChecklist)
        {
            // For Self assessment checklist the submission window is 1st January -> 31st December so we are always looking at the financial year ending last calendar year   
            latestFinancialYearToDisplay = today.Year - 1;
        }
        else
        {
            // For all other doc types, the submission window is 1st October -> 30th September
            latestFinancialYearToDisplay = today.Month >= 10 ? today.Year : today.Year - 1;
        }

        return
        [
            (new FinancialYear(latestFinancialYearToDisplay), FinancialYearSubmissionWindowStatus.Open),
            (new FinancialYear(latestFinancialYearToDisplay - 1), FinancialYearSubmissionWindowStatus.Closed),
            (new FinancialYear(latestFinancialYearToDisplay - 2), FinancialYearSubmissionWindowStatus.Closed)
        ];
    }

    public async Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(string uid,
        FinancialDocumentType financialDocumentType)
    {
        var (allFinancialDocumentsForTrust, trustOpenDate) =
            await trustDocumentRepository.GetFinancialDocumentsAsync(uid, financialDocumentType);
        var financialYearsToDisplay = GetFinancialYearsToDisplay(financialDocumentType);

        var financialDocumentServiceModels = financialYearsToDisplay.Select(year =>
        {
            var financialDocument =
                allFinancialDocumentsForTrust.SingleOrDefault(d => d.FinancialYear == year.FinancialYear);

            if (financialDocument != null)
            {
                return new FinancialDocumentServiceModel(year.FinancialYear, FinancialDocumentStatus.Submitted,
                    financialDocument.Link);
            }

            var financialDocumentStatus = year switch
            {
                { FinancialYear.End: var yearEnd } when yearEnd < trustOpenDate => FinancialDocumentStatus.NotExpected,
                { SubmissionWindowStatus: FinancialYearSubmissionWindowStatus.Open } => FinancialDocumentStatus
                    .NotYetSubmitted,
                _ => FinancialDocumentStatus.NotSubmitted
            };

            return new FinancialDocumentServiceModel(year.FinancialYear, financialDocumentStatus);
        });

        return financialDocumentServiceModels.ToArray();
    }
}
