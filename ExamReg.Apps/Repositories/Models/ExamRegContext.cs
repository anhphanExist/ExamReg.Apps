using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRegContext : DbContext
    {
        public ExamRegContext()
        {
        }

        public ExamRegContext(DbContextOptions<ExamRegContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExamPeriod> ExamPeriod { get; set; }
        public virtual DbSet<ExamProgram> ExamProgram { get; set; }
        public virtual DbSet<ExamRoom> ExamRoom { get; set; }
        public virtual DbSet<ExamRoomExamPeriod> ExamRoomExamPeriod { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentExamPeriod> StudentExamPeriod { get; set; }
        public virtual DbSet<StudentExamRoom> StudentExamRoom { get; set; }
        public virtual DbSet<StudentTerm> StudentTerm { get; set; }
        public virtual DbSet<Term> Term { get; set; }
        public virtual DbSet<User> User { get; set; }

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

            modelBuilder.Entity<ExamPeriod>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("examperiod_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.ExamDate).HasColumnType("date");
            });

            modelBuilder.Entity<ExamProgram>(entity =>
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

            modelBuilder.Entity<ExamRoom>(entity =>
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

            modelBuilder.Entity<ExamRoomExamPeriod>(entity =>
            {
                entity.HasKey(e => new { e.ExamRoomId, e.ExamPeriodId })
                    .HasName("examroomexamperiod_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("examroomexamperiod_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("semester_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Student>(entity =>
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

            modelBuilder.Entity<StudentExamPeriod>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ExamPeriodId })
                    .HasName("studentexamperiod_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentexamperiod_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentExamRoom>(entity =>
            {
                entity.HasKey(e => new { e.ExamRoomId, e.StudentId })
                    .HasName("studentexamroom_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentexamroom_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentTerm>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.TermId })
                    .HasName("studentterm_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentterm_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Term>(entity =>
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

            modelBuilder.Entity<User>(entity =>
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
        }
    }
}
