using DfE.FIAT.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<ApplicationEvent> ApplicationEvents { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingApplicationEvents(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationEvent>(entity =>
        {
            entity.ToTable("ApplicationEvent", "ops");
            entity.HasKey(e => e.Id)
                .HasName("ID");
            entity.Property(e => e.DateTime);
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Level);
            entity.Property(e => e.Code);
            entity.Property(e => e.Severity)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Description);
            entity.Property(e => e.Message);
            entity.Property(e => e.Trace);
            entity.Property(e => e.ProcessID);
            entity.Property(e => e.LineNumber);
        });
    }
}
