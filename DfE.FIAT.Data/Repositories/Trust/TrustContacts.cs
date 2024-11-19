namespace DfE.FIAT.Data.Repositories.Trust;

public record TrustContacts(
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
