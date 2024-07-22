using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public interface IAcademiesDbContext
{
    DbSet<GiasEstablishment> GiasEstablishments { get; }
    DbSet<GiasGovernance> GiasGovernances { get; }
    DbSet<GiasGroupLink> GiasGroupLinks { get; }
    DbSet<GiasGroup> Groups { get; }
    DbSet<MstrTrust> MstrTrusts { get; }
    DbSet<CdmAccount> CdmAccounts { get; }
    DbSet<MisEstablishment> MisEstablishments { get; }
    DbSet<MisFurtherEducationEstablishment> MisFurtherEducationEstablishments { get; }
    DbSet<CdmSystemuser> CdmSystemusers { get; }
    DbSet<MstrTrustGovernance> MstrTrustGovernances { get; }
    DbSet<ApplicationEvent> ApplicationEvents { get; }
    DbSet<ApplicationSetting> ApplicationSettings { get; }
}

[ExcludeFromCodeCoverage]
public partial class AcademiesDbContext : DbContext, IAcademiesDbContext
{
    public AcademiesDbContext()
    {
    }

    public AcademiesDbContext(DbContextOptions<AcademiesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingGiasEstablishments(modelBuilder);
        OnModelCreatingGiasGovernances(modelBuilder);
        OnModelCreatingGiasGroupLink(modelBuilder);
        OnModelCreatingGiasGroup(modelBuilder);
        OnModelCreatingCdmAccounts(modelBuilder);
        OnModelCreatingMisEstablishment(modelBuilder);
        OnModelCreatingCdmSystemusers(modelBuilder);
        OnModelCreatingMisFurtherEducationEstablishments(modelBuilder);
        OnModelCreatingMstrTrustGovernances(modelBuilder);
        OnModelCreatingMstrTrusts(modelBuilder);
        OnModelCreatingApplicationSettings(modelBuilder);
        OnModelCreatingApplicationEvents(modelBuilder);
        OnModelCreatingAddFilters(modelBuilder);
    }
}
