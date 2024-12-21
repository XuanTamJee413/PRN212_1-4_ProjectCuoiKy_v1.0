using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectCuoiKy.Models;

public partial class Prn212examContext : DbContext
{
    public static Prn212examContext Ins = new Prn212examContext();
    public Prn212examContext()
    {
        if (Ins == null) Ins = this;
    }

    public Prn212examContext(DbContextOptions<Prn212examContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("value")); }
    }
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseSqlServer("Data Source=TAMJEE\\SQLEXPRESS;Initial Catalog=PRN212Exam; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A752D69381");

            entity.Property(e => e.CourseName).HasMaxLength(100);

            entity.HasOne(d => d.Creator).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__Creator__3D5E1FD2");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACBEEF7B16");

            entity.Property(e => e.CorrectAnswer).HasMaxLength(255);
            entity.Property(e => e.WrongAnswer1).HasMaxLength(255);
            entity.Property(e => e.WrongAnswer2).HasMaxLength(255);
            entity.Property(e => e.WrongAnswer3).HasMaxLength(255);

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Questions__TestI__45F365D3");
        });

        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__StudentA__D48250044548D9F4");

            entity.Property(e => e.ChosenAnswer).HasMaxLength(255);

            entity.HasOne(d => d.Question).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__Quest__4AB81AF0");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__Stude__49C3F6B7");

            entity.HasOne(d => d.Test).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__TestI__48CFD27E");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Tests__8CC33160B8E99DE3");

            entity.HasIndex(e => e.TestKey, "UQ__Tests__585DF17495ACFBFC").IsUnique();

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.TestKey).HasMaxLength(50);
            entity.Property(e => e.TestName).HasMaxLength(250);

            entity.HasOne(d => d.Course).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__CourseId__412EB0B6");

            entity.HasOne(d => d.Creator).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__CreatorId__4222D4EF");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__TestResu__976902084EA3D2E6");

            entity.Property(e => e.CompletionTime).HasColumnType("datetime");
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Student).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestResul__Stude__4E88ABD4");

            entity.HasOne(d => d.Test).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestResul__TestI__4D94879B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8D1D8F1D");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4E308B09F").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasDefaultValue("Student");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
