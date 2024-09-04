using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public record TrustGovernanceServiceModel(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers);
