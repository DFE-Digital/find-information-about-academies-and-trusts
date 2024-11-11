using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustGovernanceServiceModel(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers,
    decimal TurnoverRate);
