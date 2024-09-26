namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

public record TrustContacts(
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
