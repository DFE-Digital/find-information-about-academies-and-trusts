﻿using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Converters;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<MstrAcademyConversion> MstrAcademyConversions { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMstrAcademyConversions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstrAcademyConversion>(entity =>
        {
            entity
                .HasKey(e => e.SK);

            entity.ToTable("AcademyConversions", "mstr");

            entity.Property(e => e.ProjectID)
                .HasColumnName("Project ID");

            entity.Property(e => e.TrustID)
                .HasColumnName("Trust ID");

            entity.Property(e => e.ProjectName)
                .HasColumnName("Project Name");

            entity.Property(e => e.ProjectStatus)
                .HasColumnName("Project Status");

            entity.Property(e => e.DaoProgress)
                .HasColumnName("dAO Progress");

            entity.Property(e => e.StatutoryLowestAge)
                .HasColumnName("Statutory Lowest Age");

            entity.Property(e => e.StatutoryHighestAge)
                .HasColumnName("Statutory Highest Age");

            entity.Property(e => e.LocalAuthority)
                .HasColumnName("Local Authority");

            entity.Property(e => e.ProjectApplicationType)
                .HasColumnName("Project Application Type");

            entity.Property(e => e.ExpectedOpeningDate)
                .HasColumnName("Expected Opening Date");

            entity.Property(e => e.LastDataRefresh)
                .HasColumnName("Last Data Refresh");

            entity.Property(e => e.InPrepare)
                .HasColumnName("InPrepare")
                .HasConversion<YesNoValueToBooleanConverter>();

            entity.Property(e => e.InComplete)
                .HasColumnName("InComplete")
                .HasConversion<YesNoValueToBooleanConverter>();
        });
    }
}
