using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public record AcademiesDbData(GiasEstablishment[] GiasEstablishments, Governance[] Governances,
    GroupLink[] GroupLinks,
    Group[] Groups, MstrTrust[] MstrTrusts);
