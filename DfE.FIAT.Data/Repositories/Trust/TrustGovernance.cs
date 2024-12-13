namespace DfE.FIAT.Data.Repositories.Trust;

public record TrustGovernance(
    Governor[] CurrentTrustLeadership,
    Governor[] CurrentMembers,
    Governor[] CurrentTrustees,
    Governor[] HistoricMembers);
