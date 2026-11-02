using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(e =>
            {
                e.HasIndex(d => d.Name).IsUnique();

                e.HasData(
                    new Department { Id = 1, Name = "علوم الحاسب" },
                    new Department { Id = 2, Name = "الهندسة" },
                    new Department { Id = 3, Name = "إدارة الأعمال" },
                    new Department { Id = 4, Name = "الصيدلة" }
                );
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.Property(s => s.Gpa).HasPrecision(3, 2);   // مثال: 3.75
                e.HasIndex(s => s.Email).IsUnique();
                e.HasIndex(s => s.FullName);

                e.HasOne(s => s.Department)
                 .WithMany(d => d.Students)
                 .HasForeignKey(s => s.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);        // ممنوع حذف قسم فيه طلاب

                e.HasData(SeedStudents());
            });
        }

        // 45 طالب تجريبي عشان نجرب الـ Pagination (قيم ثابتة مش عشوائية)
        private static List<Student> SeedStudents()
        {
            string[] firstNames = { "أحمد", "محمد", "منى", "سارة", "يوسف", "نور", "عمر", "هبة", "كريم", "ليلى" };
            string[] lastNames = { "حسن", "علي", "إبراهيم", "السيد", "محمود", "فتحي" };

            var students = new List<Student>();
            for (int i = 1; i <= 45; i++)
            {
                students.Add(new Student
                {
                    Id = i,
                    FullName = $"{firstNames[i % firstNames.Length]} {lastNames[i % lastNames.Length]}",
                    Email = $"student{i}@example.com",
                    BirthDate = new DateTime(2000 + i % 5, i % 12 + 1, i % 28 + 1),
                    Gpa = 2.0m + (i % 21) * 0.1m,
                    EnrollmentDate = new DateTime(2023 + i % 3, 9, 1),
                    DepartmentId = i % 4 + 1
                });
            }
            return students;
        }
    }
}