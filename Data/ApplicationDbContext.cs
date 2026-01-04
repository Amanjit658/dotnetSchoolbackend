using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myFirstSchoolProject.Models;

namespace myFirstSchoolProject.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        // 🔽 ADD DbSets HERE
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ClassSubject> ClassSubjects { get; set; }

        public DbSet<TeacherClassSubject> TeacherClassSubjects { get; set; }

        public DbSet<ClassTimetable> ClassTimetables { get; set; }

        public DbSet<AcademicYear> AcademicYears { get; set; }

        public DbSet<StudentClassAllocation> StudentClassAllocations { get; set; }

        public DbSet<StudentAttendance> StudentAttendances { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamSubject> ExamSubjects { get; set; }
      public DbSet<StudentExamResult> StudentExamResults { get; set; }


          protected override void OnModelCreating(ModelBuilder builder)
          {
            base.OnModelCreating(builder);

            // Prevent multiple cascade paths by restricting deletes from principal
            builder.Entity<StudentAttendance>(entity =>
            {
                entity.HasOne(sa => sa.Student)
                    .WithMany()
                    .HasForeignKey(sa => sa.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Class)
                    .WithMany()
                    .HasForeignKey(sa => sa.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Subject)
                    .WithMany()
                    .HasForeignKey(sa => sa.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Teacher)
                    .WithMany()
                    .HasForeignKey(sa => sa.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
          }
    }
}
