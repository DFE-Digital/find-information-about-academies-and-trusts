using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public interface IAcademiesDbContext
{
    DbSet<GiasEstablishment> GiasEstablishments { get; }
    DbSet<GiasGovernance> GiasGovernances { get; }
    DbSet<GiasGroupLink> GiasGroupLinks { get; }
    DbSet<GiasGroup> Groups { get; }
    DbSet<GiasEstablishmentLink> GiasEstablishmentLink { get; }
    DbSet<MstrTrust> MstrTrusts { get; }
    DbSet<MstrFreeSchoolProject> MstrFreeSchoolProjects { get; }
    DbSet<MstrAcademyConversion> MstrAcademyConversions { get; }
    DbSet<MstrAcademyTransfer> MstrAcademyTransfers { get; }
    DbSet<TadTrustGovernance> TadTrustGovernances { get; }
    DbSet<ApplicationEvent> ApplicationEvents { get; }
    DbSet<ApplicationSetting> ApplicationSettings { get; }
    DbSet<MisMstrEstablishmentFiat> MisMstrEstablishmentsFiat { get; }
    DbSet<MisMstrFurtherEducationEstablishmentFiat> MisMstrFurtherEducationEstablishmentsFiat { get; }
    DbSet<SharepointTrustDocLink> SharepointTrustDocLinks { get; }
    
    DbSet<TadHeadTeacherContactsFiat> TadHeadTeacherContacts { get; }
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
        OnModelCreatingApplicationEvents(modelBuilder);
        OnModelCreatingApplicationSettings(modelBuilder);
        OnModelCreatingGiasEstablishmentLink(modelBuilder);
        OnModelCreatingGiasEstablishments(modelBuilder);
        OnModelCreatingGiasGovernances(modelBuilder);
        OnModelCreatingGiasGroup(modelBuilder);
        OnModelCreatingGiasGroupLink(modelBuilder);
        OnModelCreatingMis_Mstr(modelBuilder);
        OnModelCreatingMstrAcademyConversions(modelBuilder);
        OnModelCreatingMstrAcademyTransfers(modelBuilder);
        OnModelCreatingMstrFreeSchoolProjects(modelBuilder);
        OnModelCreatingMstrTrusts(modelBuilder);
        OnModelCreatingSharePointTrustDocLinks(modelBuilder);
        OnModelCreatingTadTrustGovernances(modelBuilder);
        OnModelCreatingTadHeadTeacherContacts(modelBuilder);
    }
}
