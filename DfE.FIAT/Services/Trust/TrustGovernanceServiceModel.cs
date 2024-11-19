using DfE.FIAT.Data.Repositories.Trust;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Web.Services.Trust;

[ExcludeFromCodeCoverage]
public record TrustGovernanceServiceModel(
    Governor[] CurrentTrustLeadership,
    Governor[] CurrentMembers,
    Governor[] CurrentTrustees,
    Governor[] HistoricMembers,
    decimal TurnoverRate);