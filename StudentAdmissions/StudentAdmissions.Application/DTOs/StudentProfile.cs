using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.DTOs
{
    public class StudentProfile
    {
        public Student Student { get; set; }
        public List<StudentPower> StudentPowers { get; set; }
        public List<StudentWeakness> StudentWeaknesses { get; set; }
        public List<StudentSection> Sections { get; set; }
    }
}
