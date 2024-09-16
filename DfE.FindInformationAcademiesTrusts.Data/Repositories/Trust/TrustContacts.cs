namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

public record TrustContacts(
    Person? TrustRelationshipManager,
    Person? SfsoLead,
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
