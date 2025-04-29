using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<GiasEstablishmentLink> GiasEstablishmentLink { get; set; }
    
    public static readonly Expression<Func<GiasEstablishmentLink, bool>> GiasEstablishmentLinkQueryFilter =
        gel => gel.Urn != null
               && gel.LinkUrn != null
               && gel.LinkType != null;
    
    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingGiasEstablishmentLink(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasEstablishmentLink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EstablishmentLink", "gias");

            entity.Property(e => e.Urn)
                .IsUnicode(false)
                .HasColumnName("URN");
            entity.Property(e => e.LinkUrn)
                .IsUnicode(false)
                .HasColumnName("LinkURN");
            entity.Property(e => e.LinkName).IsUnicode(false);
            entity.Property(e => e.LinkType).IsUnicode(false);
            entity.Property(e => e.LinkEstablishedDate).IsUnicode(false);

            entity.HasQueryFilter(GiasEstablishmentLinkQueryFilter);
        });
    }
}
