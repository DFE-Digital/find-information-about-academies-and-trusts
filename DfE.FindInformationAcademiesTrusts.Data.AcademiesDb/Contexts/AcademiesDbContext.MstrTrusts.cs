﻿using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<MstrTrust> MstrTrusts { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMstrTrusts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstrTrust>(entity =>
        {
            entity
                .HasKey(e => e.GroupUid);
            entity.ToTable("Trust", "mstr");

            entity.Property(e => e.GroupUid)
                .IsUnicode(false)
                .ValueGeneratedNever()
                .HasColumnName("Group UID");

            entity.Property(e => e.GORregion)
                .IsUnicode(false)
                .HasColumnName("GORregion");
        });
    }
}