using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustGovernanceServiceModel(
    Governor[] CurrentTrustLeadership,
    Governor[] CurrentMembers,
    Governor[] CurrentTrustees,
    Governor[] HistoricMembers,
    decimal TurnoverRate);
