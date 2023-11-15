using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public record AcademiesDbData(Establishment[] Establishments, Governance[] Governances, GroupLink[] GroupLinks,
    Group[] Groups, MstrTrust[] MstrTrusts);
