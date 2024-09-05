using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingAddFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasGroup>(entity =>
        {
            // This filters out all groups with invalid fields
            // Also filters to only MATs and SATs
            // filters out all closed groups (trusts)
            entity.HasQueryFilter(g =>
                g.GroupUid != null &&
                g.GroupId != null &&
                g.GroupName != null &&
                g.GroupType != null &&
                g.GroupStatusCode == "OPEN" &&
                (g.GroupType == "Multi-academy trust" ||
                 g.GroupType == "Single-academy trust"));
        });

        modelBuilder.Entity<GiasGroupLink>(entity =>
        {
            entity.HasQueryFilter(gl =>
                gl.Urn != null);
        });

        modelBuilder.Entity<MisEstablishment>(entity =>
        {
            entity.HasQueryFilter(gl =>
                gl.Urn != null);
        });
    }
}
