using StudentAdmissions.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application
{
    public class AcademyContext : DbContext
    {
        public DbSet<Student> Student { get; set; }
        public DbSet<Weakness> Weakness { get; set; }

        public DbSet<Power> Power { get; set; }
        public DbSet<StudentPower> StudentPower { get; set; }
        public DbSet<StudentWeakness> StudentWeakness { get; set; }
        public DbSet<PowerType> PowerType { get; set; }
        public DbSet<WeaknessType> WeaknessType { get; set; }
        public DbSet<StudentSection> StudentSection { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Instructor> Instructor { get; set; }

        public AcademyContext(DbContextOptions options) : base(options)
        {
        }
    }
}
