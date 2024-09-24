using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustContactsServiceModel(
    InternalContact? TrustRelationshipManager,
    InternalContact? SfsoLead,
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
