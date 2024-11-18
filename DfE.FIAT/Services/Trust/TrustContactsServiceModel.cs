using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Services.Trust;

public record TrustContactsServiceModel(
    InternalContact? TrustRelationshipManager,
    InternalContact? SfsoLead,
    Person? AccountingOfficer,
    Person? ChairOfTrustees,
    Person? ChiefFinancialOfficer
);
