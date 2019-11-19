using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRegContext : DbContext
    {
        public virtual DbSet<ExamPeriodDAO> ExamPeriod { get; set; }
        public virtual DbSet<ExamProgramDAO> ExamProgram { get; set; }
        public virtual DbSet<ExamRoomDAO> ExamRoom { get; set; }
        public virtual DbSet<ExamRoomExamPeriodDAO> ExamRoomExamPeriod { get; set; }
        public virtual DbSet<SemesterDAO> Semester { get; set; }
        public virtual DbSet<StudentDAO> Student { get; set; }
        public virtual DbSet<StudentExamPeriodDAO> StudentExamPeriod { get; set; }
        public virtual DbSet<StudentExamRoomDAO> StudentExamRoom { get; set; }
        public virtual DbSet<StudentTermDAO> StudentTerm { get; set; }
        public virtual DbSet<TermDAO> Term { get; set; }
        public virtual DbSet<UserDAO> User { get; set; }

        public ExamRegContext(DbContextOptions<ExamRegContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=ExamReg;Username=postgres;Password=12345678");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ExamPeriodDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("examperiod_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.ExamDate).HasColumnType("date");
            });

            modelBuilder.Entity<ExamProgramDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("examprogram_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ExamRoomDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("examroom_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AmphitheaterName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ExamRoomExamPeriodDAO>(entity =>
            {
                entity.HasKey(e => new { e.ExamRoomId, e.ExamPeriodId })
                    .HasName("examroomexamperiod_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("examroomexamperiod_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SemesterDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("semester_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentDAO>(entity =>
            {
                entity.HasIndex(e => new { e.StudentNumber, e.CX })
                    .HasName("student_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.GivenName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<StudentExamPeriodDAO>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ExamPeriodId })
                    .HasName("studentexamperiod_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentexamperiod_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentExamRoomDAO>(entity =>
            {
                entity.HasKey(e => new { e.ExamRoomId, e.StudentId })
                    .HasName("studentexamroom_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentexamroom_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentTermDAO>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.TermId })
                    .HasName("studentterm_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentterm_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TermDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("term_cx_idx")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("user_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);
    }
}
