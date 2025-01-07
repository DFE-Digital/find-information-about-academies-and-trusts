using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
public partial class AcademiesDbContext
{
    public DbSet<MstrFreeSchoolProject> MstrFreeSchoolProjects { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMstrFreeSchoolProjects(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstrFreeSchoolProject>(entity =>
        {
            entity.ToTable("FreeSchoolProjects", "mstr");
        });
    }
}
