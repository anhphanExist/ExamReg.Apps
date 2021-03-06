﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRegContext : DbContext
    {
        public virtual DbSet<ExamPeriodDAO> ExamPeriod { get; set; }
        public virtual DbSet<ExamProgramDAO> ExamProgram { get; set; }
        public virtual DbSet<ExamRegisterDAO> ExamRegister { get; set; }
        public virtual DbSet<ExamRoomDAO> ExamRoom { get; set; }
        public virtual DbSet<ExamRoomExamPeriodDAO> ExamRoomExamPeriod { get; set; }
        public virtual DbSet<SemesterDAO> Semester { get; set; }
        public virtual DbSet<StudentDAO> Student { get; set; }
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

                entity.HasOne(d => d.ExamProgram)
                    .WithMany(p => p.ExamPeriods)
                    .HasForeignKey(d => d.ExamProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examperiod_fk_1");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.ExamPeriods)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examperiod_fk");
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

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.ExamPrograms)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examprogram_fk");
            });

            modelBuilder.Entity<ExamRegisterDAO>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ExamRoomId, e.ExamPeriodId })
                    .HasName("examregister_pk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ExamRegisters)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examregister_fk_1");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamRegisters)
                    .HasForeignKey(d => new { d.ExamRoomId, d.ExamPeriodId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examregister_fk");
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

                entity.HasOne(d => d.ExamPeriod)
                    .WithMany(p => p.ExamRoomExamPeriods)
                    .HasForeignKey(d => d.ExamPeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examroomexamperiod_fk_1");

                entity.HasOne(d => d.ExamRoom)
                    .WithMany(p => p.ExamRoomExamPeriods)
                    .HasForeignKey(d => d.ExamRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examroomexamperiod_fk");
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

            modelBuilder.Entity<StudentTermDAO>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.TermId })
                    .HasName("studentterm_pk");

                entity.HasIndex(e => e.CX)
                    .HasName("studentterm_un")
                    .IsUnique();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentTerms)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_studentterm_fk");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.StudentTerms)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("term_studentterm_fk");
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

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Terms)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("term_fk");
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

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("user_fk");
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);
    }
}
