using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.ServiceModels;

public record TrustGovernanceServiceModel(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers);
