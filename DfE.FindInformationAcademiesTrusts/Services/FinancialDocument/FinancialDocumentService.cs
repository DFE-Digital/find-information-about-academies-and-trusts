using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

namespace DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

public interface IFinancialDocumentService
{
    Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(string trustReferenceNumber,
        FinancialDocumentType financialDocumentType);
}

public class FinancialDocumentService(ITrustDocumentRepository trustDocumentRepository) : IFinancialDocumentService
{
    public enum FinancialYearSubmissionWindowStatus
    {
        Open,
        Closed
    }

    public static (FinancialYear FinancialYear, FinancialYearSubmissionWindowStatus SubmissionWindowStatus)[]
        GetFinancialYearsToDisplay(FinancialDocumentType financialDocumentType)
    {
        // A financial year runs from 1st September to 31st August
        // The submission window for financial documents starts after that year has ended (and varies by financial document type)
        // We display the latest financial year with an open submission window and the two years before that

        //Todo: Use datetime provider
        var today = DateTime.Today;

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

    public async Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(string trustReferenceNumber,
        FinancialDocumentType financialDocumentType)
    {
        //Todo: when did trust open?
        var trustOpenDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2));

        //Get all the financial docs 
        var docs = await trustDocumentRepository.GetFinancialDocumentsAsync(trustReferenceNumber,
            financialDocumentType);

        //Get years to display
        var financialYearsToDisplay = GetFinancialYearsToDisplay(financialDocumentType);

        //Transform
        var financialDocumentServiceModels = financialYearsToDisplay.Select(year =>
        {
            var link = docs.SingleOrDefault(d => d.FinancialYearEnd == year.FinancialYear.End);

            if (link != null)
            {
                return new FinancialDocumentServiceModel(year.FinancialYear, FinancialDocumentStatus.Submitted,
                    link.DocumentLink);
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
