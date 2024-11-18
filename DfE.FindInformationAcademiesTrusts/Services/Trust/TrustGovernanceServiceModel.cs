using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

[ExcludeFromCodeCoverage]
public record TrustGovernanceServiceModel(
    Governor[] CurrentTrustLeadership,
    Governor[] CurrentMembers,
    Governor[] CurrentTrustees,
    Governor[] HistoricMembers,
    decimal TurnoverRate);