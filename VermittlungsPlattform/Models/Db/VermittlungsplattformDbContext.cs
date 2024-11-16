using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VermittlungsPlattform.Models.Db;

public partial class VermittlungsplattformDbContext : DbContext
{
    public VermittlungsplattformDbContext()
    {
    }

    public VermittlungsplattformDbContext(DbContextOptions<VermittlungsplattformDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }

    public virtual DbSet<CompanyProfileGallery> CompanyProfileGalleries { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=FANTOMAS007\\SQLEXPRESS;Database=VermittlungsplattformDb;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyProfile>(entity =>
        {
            entity.ToTable("CompanyProfile");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Branche)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Description)
                .HasMaxLength(3000)
                .IsFixedLength();
            entity.Property(e => e.Imagename)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Link)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Webseite)
                .HasMaxLength(200)
                .IsFixedLength();
        });

        modelBuilder.Entity<CompanyProfileGallery>(entity =>
        {
            entity.ToTable("CompanyProfileGallery");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyProfileId).HasColumnName("CompanyProfileID");
            entity.Property(e => e.ImageName)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Link)
                .HasMaxLength(300)
                .IsFixedLength();
            entity.Property(e => e.MenuTitle)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
