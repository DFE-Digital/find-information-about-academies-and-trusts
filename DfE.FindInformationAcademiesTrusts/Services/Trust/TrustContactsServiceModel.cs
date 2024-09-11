using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustContactsServiceModel(
    Person? TrustRelationshipManager,
    Person? SfsoLead,
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
