using System;
using System.Collections.Generic;
using ConsoleAppTestDb.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppTestDb.Data;

public partial class EnglishVocabularyQuizContext : DbContext
{
    public EnglishVocabularyQuizContext()
    {
    }

    public EnglishVocabularyQuizContext(DbContextOptions<EnglishVocabularyQuizContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=TAMJEE\\SQLEXPRESS;Initial Catalog=EnglishVocabularyQuiz; Trusted_Connection=SSPI;Encrypt=false;User ID=sa;Password=123456;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD9E38DCD");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F2845622C6E786").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.HasKey(e => e.WordId).HasName("PK__Vocabula__2C20F046DB2B13AF");

            entity.ToTable("Vocabulary");

            entity.Property(e => e.WordId).HasColumnName("WordID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EnglishWord).HasMaxLength(255);
            entity.Property(e => e.VietnameseWord).HasMaxLength(255);
            entity.Property(e => e.WordType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
