using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public interface IAcademiesDbContext
{
    DbSet<GiasEstablishment> GiasEstablishments { get; }
    DbSet<GiasGovernance> GiasGovernances { get; }
    DbSet<GiasGroupLink> GiasGroupLinks { get; }
    DbSet<GiasGroup> Groups { get; }
    DbSet<MstrTrust> MstrTrusts { get; }
    DbSet<MstrFreeSchoolProject> MstrFreeSchoolProjects { get; }
    DbSet<MstrAcademyConversions> MstrAcademyConversions { get; }
    DbSet<MstrAcademyTransfers> MstrAcademyTransfers { get; set; }
    DbSet<CdmAccount> CdmAccounts { get; }
    DbSet<CdmSystemuser> CdmSystemusers { get; }
    DbSet<TadTrustGovernance> TadTrustGovernances { get; }
    DbSet<ApplicationEvent> ApplicationEvents { get; }
    DbSet<ApplicationSetting> ApplicationSettings { get; }
    DbSet<MisMstrEstablishmentFiat> MisMstrEstablishmentsFiat { get; }
    DbSet<MisMstrFurtherEducationEstablishmentFiat> MisMstrFurtherEducationEstablishmentsFiat { get; }
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
        OnModelCreatingCdmSystemusers(modelBuilder);
        OnModelCreatingTadTrustGovernances(modelBuilder);
        OnModelCreatingMstrTrusts(modelBuilder);
        OnModelCreatingMstrFreeSchoolProjects(modelBuilder);
        OnModelCreatingMstrAcademyConversions(modelBuilder);
        OnModelCreatingMstrAcademyTransfers(modelBuilder);
        OnModelCreatingApplicationSettings(modelBuilder);
        OnModelCreatingApplicationEvents(modelBuilder);
        OnModelCreatingAddFilters(modelBuilder);
        OnModelCreatingMis_Mstr(modelBuilder);
    }
}
