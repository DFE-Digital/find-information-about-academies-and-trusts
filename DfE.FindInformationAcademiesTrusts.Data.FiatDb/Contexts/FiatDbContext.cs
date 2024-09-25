﻿using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;

[ExcludeFromCodeCoverage(Justification = "Difficult to unit test")]
public sealed class FiatDbContext(
    DbContextOptions<FiatDbContext> options,
    SetChangedByInterceptor setChangedByInterceptor)
    : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(setChangedByInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var contactEntity = modelBuilder
            .Entity<Contact>();

        contactEntity
            .ToTable("Contacts", table => table.IsTemporal());

        contactEntity
            .HasIndex(c => c.Uid);

        contactEntity
            .HasIndex(c => new { TrustUid = c.Uid, c.Role })
            .IsUnique();

        contactEntity.Property(c => c.Role)
            .HasConversion<string>();

        contactEntity.Property(c => c.LastModifiedAtTime)
            .HasComputedColumnSql("[PeriodStart]");
    }
}
