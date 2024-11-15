using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectCuoiKy.Models;

public partial class Prn212quizDbContext : DbContext
{
    public static Prn212quizDbContext Ins = new Prn212quizDbContext();
    public Prn212quizDbContext()
    {
        if (Ins == null) Ins = this;
    }

    public Prn212quizDbContext(DbContextOptions<Prn212quizDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection")); }

    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//    => optionsBuilder.UseSqlServer("Data Source=TAMJEE\\SQLEXPRESS;Initial Catalog=PRN212QuizDb; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7F17FA455");

            entity.Property(e => e.CourseName).HasMaxLength(100);

            entity.HasOne(d => d.Creator).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__Creator__3D5E1FD2");
        });

        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__StudentA__D48250042DADD4B8");

            entity.Property(e => e.ChosenAnswer).HasMaxLength(255);

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__Stude__4D94879B");

            entity.HasOne(d => d.Term).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.TermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__TermI__4E88ABD4");

            entity.HasOne(d => d.Test).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentAn__TestI__4CA06362");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.TermId).HasName("PK__Terms__410A21A50FA3A3E3");

            entity.Property(e => e.CorrectAnswer).HasMaxLength(255);
            entity.Property(e => e.TermText).HasMaxLength(100);
            entity.Property(e => e.WrongAnswer1).HasMaxLength(255);
            entity.Property(e => e.WrongAnswer2).HasMaxLength(255);
            entity.Property(e => e.WrongAnswer3).HasMaxLength(255);

            entity.HasOne(d => d.Course).WithMany(p => p.Terms)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Terms__CourseId__403A8C7D");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Tests__8CC331604F737605");

            entity.HasIndex(e => e.TestKey, "UQ__Tests__585DF174427A29DE").IsUnique();

            entity.Property(e => e.TestKey).HasMaxLength(50);
            entity.Property(e => e.TimerEnabled).HasDefaultValue(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__CourseId__44FF419A");

            entity.HasOne(d => d.Creator).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__CreatorId__45F365D3");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__TestResu__97690208C7DE2E10");

            entity.Property(e => e.CompletionTime).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestResul__Stude__49C3F6B7");

            entity.HasOne(d => d.Test).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TestResul__TestI__48CFD27E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8681292B");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4205C16A6").IsUnique();

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
