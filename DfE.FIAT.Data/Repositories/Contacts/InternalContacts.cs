namespace DfE.FIAT.Data.Repositories.Contacts;

public record InternalContacts(
    InternalContact? TrustRelationshipManager,
    InternalContact? SfsoLead
);
