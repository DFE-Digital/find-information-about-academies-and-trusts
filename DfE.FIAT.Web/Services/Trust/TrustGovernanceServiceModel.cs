using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data.Repositories.Trust;

namespace DfE.FIAT.Web.Services.Trust;

[ExcludeFromCodeCoverage]
public record TrustGovernanceServiceModel(
    Governor[] CurrentTrustLeadership,
    Governor[] CurrentMembers,
    Governor[] CurrentTrustees,
    Governor[] HistoricMembers,
    decimal TurnoverRate);