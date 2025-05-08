using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;

public static class IQueryableExtensions
{
    private const string MultiAcademyTrust = "Multi-academy trust";
    private const string SingleAcademyTrust = "Single-academy trust";

    public static IQueryable<GiasGroup> Trusts(this IQueryable<GiasGroup> giasGroups)
    {
        return giasGroups.Where(g =>
            g.GroupId != null &&
            (g.GroupType == MultiAcademyTrust || g.GroupType == SingleAcademyTrust)
        );
    }
    
    public static IQueryable<GiasGroupLink> Trusts(this IQueryable<GiasGroupLink> giasGroups)
    {
        return giasGroups.Where(g =>
            g.GroupId != null &&
            (g.GroupType == MultiAcademyTrust || g.GroupType == SingleAcademyTrust)
        );
    }

    public static IQueryable<GiasGroupLink> SingleAcademyTrusts(this IQueryable<GiasGroupLink> giasGroups)
    {
        return giasGroups.Where(g =>
            g.GroupId != null &&
            g.GroupType == SingleAcademyTrust);
    }
}
