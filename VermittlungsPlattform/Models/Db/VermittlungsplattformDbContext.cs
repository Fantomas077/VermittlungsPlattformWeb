﻿using System;
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

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CompanyGallery> CompanyGalleries { get; set; }

    public virtual DbSet<CompanyInteresse> CompanyInteresses { get; set; }

    public virtual DbSet<Interesse> Interesses { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<PraktikumStelle> PraktikumStelles { get; set; }


    public virtual DbSet<StelleBewerbung> StelleBewerbungs { get; set; }

    public virtual DbSet<StudentProfile> StudentProfiles { get; set; }

    public virtual DbSet<StudentenInteresse> StudentenInteresses { get; set; }

    public virtual DbSet<UnternehmenProfile> UnternehmenProfiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=FANTOMAS007\\SQLEXPRESS;Database=VermittlungsplattformDb;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.ToTable("Banner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Link)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.SubTitle)
                .HasMaxLength(1000)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsFixedLength();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CommentText)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UnternehmenId).HasColumnName("UnternehmenID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<CompanyGallery>(entity =>
        {
            entity.ToTable("CompanyGallery");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyProfileId).HasColumnName("CompanyProfileID");
            entity.Property(e => e.ImageName)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<CompanyInteresse>(entity =>
        {
            entity.ToTable("CompanyInteresse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UnternehmenInteresse)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UnternehmenprofilId).HasColumnName("UnternehmenprofilID");
        });

        modelBuilder.Entity<Interesse>(entity =>
        {
            entity.ToTable("Interesse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
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

        modelBuilder.Entity<PraktikumStelle>(entity =>
        {
            entity.ToTable("PraktikumStelle");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Arbeitsyp)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Branche)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.Gehalt).HasColumnType("money");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Skills)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Tags)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UnternehmenProfileId).HasColumnName("UnternehmenProfileID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });



        modelBuilder.Entity<StelleBewerbung>(entity =>
        {
            entity.ToTable("StelleBewerbung");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Anschreiben)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.ApplyDate).HasColumnType("datetime");
            entity.Property(e => e.Cv)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("CV");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.StelleId).HasColumnName("StelleID");
            entity.Property(e => e.StudentProfilId).HasColumnName("StudentProfilID");
            entity.Property(e => e.UnternhemenId).HasColumnName("UnternhemenID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.ToTable("StudentProfile");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Abschluss)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Apropos)
                .HasMaxLength(1000)
                .IsFixedLength();
            entity.Property(e => e.Cvname)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("CVName");
            entity.Property(e => e.Facebook)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Fachrichtung)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Geschlecht)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Github)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Instagram)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Linkedin)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Schwerpunkte)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Skills)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Studiengang)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Twitter)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<StudentenInteresse>(entity =>
        {
            entity.ToTable("StudentenInteresse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StudentInteresse)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.StudentprofileId).HasColumnName("StudentprofileID");
        });

        modelBuilder.Entity<UnternehmenProfile>(entity =>
        {
            entity.ToTable("UnternehmenProfile");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Branche)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Description)
                .HasMaxLength(3000)
                .IsFixedLength();
            entity.Property(e => e.ImageName)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Link)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Webseite)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
            entity.Property(e => e.Vorname)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
