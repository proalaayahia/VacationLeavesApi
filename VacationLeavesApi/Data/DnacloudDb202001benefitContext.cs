using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VacationLeavesApi.Data;

public partial class DnacloudDb202001benefitContext : DbContext
{
    public DnacloudDb202001benefitContext()
    {
    }

    public DnacloudDb202001benefitContext(DbContextOptions<DnacloudDb202001benefitContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Vacation> Vacations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=fz-dv-db01;database=DNACloudDB202001Benefit;user=hitsapp;password=123;trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.HasKey(e => e.Vaccode).HasName("PK_vacation");

            entity.ToTable("Vacation", tb =>
                {
                    tb.HasTrigger("Trig_Del_Vacation");
                    tb.HasTrigger("Trig_UPD_Vacation");
                    tb.HasTrigger("tr_delete_AUDIT_Vacation");
                    tb.HasTrigger("tr_insert_AUDIT_Vacation");
                    tb.HasTrigger("tr_update_AUDIT_Vacation");
                });

            entity.HasIndex(e => e.Legend, "IX_vacation").IsUnique();

            entity.Property(e => e.Vaccode)
                .ValueGeneratedNever()
                .HasColumnName("vaccode");
            entity.Property(e => e.DayFactor).HasColumnType("numeric(9, 3)");
            entity.Property(e => e.Legend)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("legend");
            entity.Property(e => e.PartDayFactorFrom).HasColumnType("numeric(9, 3)");
            entity.Property(e => e.PartDayFactorTo).HasColumnType("numeric(9, 3)");
            entity.Property(e => e.Postpaycd).HasColumnName("postpaycd");
            entity.Property(e => e.VacationAname)
                .HasMaxLength(60)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationAName");
            entity.Property(e => e.VacationAnameCons)
                .HasMaxLength(250)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationANameCons");
            entity.Property(e => e.VacationFname)
                .HasMaxLength(60)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationFName");
            entity.Property(e => e.VacationFnameCons)
                .HasMaxLength(250)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationFNameCons");
            entity.Property(e => e.VacationFromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VacationGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("VacationGUID");
            entity.Property(e => e.VacationName)
                .HasMaxLength(60)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.VacationNameCons)
                .HasMaxLength(250)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.VacationRdesc)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationRDesc");
            entity.Property(e => e.VacationRexecDate)
                .HasColumnType("datetime")
                .HasColumnName("VacationRExecDate");
            entity.Property(e => e.VacationRissueDate)
                .HasColumnType("datetime")
                .HasColumnName("VacationRIssueDate");
            entity.Property(e => e.VacationRno)
                .HasMaxLength(10)
                .HasDefaultValueSql("('')")
                .HasColumnName("VacationRNo");
            entity.Property(e => e.VacationRoles).HasDefaultValueSql("('')");
            entity.Property(e => e.VacationRsource).HasColumnName("VacationRSource");
            entity.Property(e => e.VacationRstatus).HasColumnName("VacationRStatus");
            entity.Property(e => e.VacationRyear).HasColumnName("VacationRYear");
            entity.Property(e => e.VacationToDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Vacbalcat).HasColumnName("vacbalcat");
            entity.Property(e => e.Vacbalcat1).HasColumnName("vacbalcat1");
            entity.Property(e => e.Vacbalcat2).HasColumnName("vacbalcat2");
            entity.Property(e => e.Vactype).HasColumnName("vactype");
        });
        modelBuilder.HasSequence("CountBy");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
