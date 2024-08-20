namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

public record TrustGoverenance(
    Governor[] TrustLeadership,
    Governor[] Members,
    Governor[] Trustees,
    Governor[] HistoricMembers);
