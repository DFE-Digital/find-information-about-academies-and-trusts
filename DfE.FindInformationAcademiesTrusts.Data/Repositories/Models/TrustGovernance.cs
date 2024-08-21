namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

public record TrustGovernance(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers);
