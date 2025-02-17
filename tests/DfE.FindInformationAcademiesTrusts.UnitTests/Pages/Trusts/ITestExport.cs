namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public interface ITestExport
{
    Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid();
    Task OnGetExportAsync_ShouldReturnNotFound_WhenUidIsInvalid();
    Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters();
}
