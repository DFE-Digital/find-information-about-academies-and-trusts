namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

public record TrustGovernance(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers);
