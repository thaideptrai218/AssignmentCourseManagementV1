using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace AssignmentCourseManagementV1.Models;

public partial class APContext : DbContext
{
    public APContext()
    {
    }

    public APContext(DbContextOptions<APContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseSchedule> CourseSchedules { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<RollCallBook> RollCallBooks { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.CampusId).HasName("PK_Campuses");

            entity.ToTable("CAMPUSES");

            entity.Property(e => e.CampusId).ValueGeneratedNever();
            entity.Property(e => e.CampusCode).HasMaxLength(50);
            entity.Property(e => e.CampusName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("COURSES");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CourseCode).HasMaxLength(50);
            entity.Property(e => e.CourseDescription).HasMaxLength(300);

            entity.HasOne(d => d.Campus).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK_COURSES_Campuses");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK_COURSES_INSTRUCTORS");

            entity.HasOne(d => d.Subject).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COURSES_SUBJECTS");

            entity.HasOne(d => d.Term).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TermId)
                .HasConstraintName("FK_COURSES_TERMS");

            entity.HasMany(d => d.Students).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_STUDENT_COURSE_STUDENTS"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_STUDENT_COURSE_COURSES"),
                    j =>
                    {
                        j.HasKey("CourseId", "StudentId");
                        j.ToTable("STUDENT_COURSE");
                    });
        });

        modelBuilder.Entity<CourseSchedule>(entity =>
        {
            entity.HasKey(e => e.TeachingScheduleId).HasName("PK_TEACHING_SCHEDULES");

            entity.ToTable("COURSE_SCHEDULES");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.TeachingDate).HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseSchedules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_COURSE_SCHEDULES_COURSES");

            entity.HasOne(d => d.Room).WithMany(p => p.CourseSchedules)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_COURSE_SCHEDULES_ROOMS");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("DEPARTMENTS");

            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(300);
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.ToTable("INSTRUCTORS");

            entity.Property(e => e.InstructorFirstName).HasMaxLength(50);
            entity.Property(e => e.InstructorLastName).HasMaxLength(50);
            entity.Property(e => e.InstructorMidName).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_INSTRUCTORS_DEPARTMENTS");
        });

        modelBuilder.Entity<RollCallBook>(entity =>
        {
            entity.ToTable("ROLL_CALL_BOOKS");

            entity.Property(e => e.Comment).HasMaxLength(300);

            entity.HasOne(d => d.Student).WithMany(p => p.RollCallBooks)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_ROLL_CALL_BOOKS_STUDENTS");

            entity.HasOne(d => d.TeachingSchedule).WithMany(p => p.RollCallBooks)
                .HasForeignKey(d => d.TeachingScheduleId)
                .HasConstraintName("FK_ROLL_CALL_BOOKS_COURSE_SCHEDULES");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("ROOMS");

            entity.Property(e => e.RoomId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.RoomCode).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Campus).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK_ROOMS_Campuses");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("STUDENTS");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MidName).HasMaxLength(50);
            entity.Property(e => e.Roll)
                .HasMaxLength(50)
                .HasColumnName("Roll#");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("SUBJECTS");

            entity.Property(e => e.SubjectCode).HasMaxLength(50);
            entity.Property(e => e.SubjectName).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_SUBJECTS_DEPARTMENTS");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.TermId).HasName("PK_Terms");

            entity.ToTable("TERMS");

            entity.Property(e => e.TermId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.TermName).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
