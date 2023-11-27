using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public class AcademiesDbData
{
    public List<CdmAccount> CdmAccounts { get; } = new();
    public List<CdmSystemuser> CdmSystemusers { get; } = new();
    public List<GiasEstablishment> GiasEstablishments { get; } = new();
    public List<GiasGovernance> GiasGovernances { get; } = new();
    public List<GiasGroupLink> GiasGroupLinks { get; } = new();
    public List<GiasGroup> GiasGroups { get; } = new();
    public List<MstrTrust> MstrTrusts { get; } = new();
}
