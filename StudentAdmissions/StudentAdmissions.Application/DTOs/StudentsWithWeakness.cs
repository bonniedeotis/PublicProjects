using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.DTOs
{
    public class StudentsWithWeakness
    {
        public string WeaknessName { get; set; }
        public List<StudentWeakness> Students { get; set; }
    }
}
