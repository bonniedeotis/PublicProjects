using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Entities
{
    [PrimaryKey(nameof(SectionID), nameof(StudentID))]

    public class StudentSection
    {
        public int SectionID { get; set; }
        public int StudentID { get; set; }
        public byte? Grade { get; set; }
        public byte? Absences { get; set; }

        public Section Section { get; set; }
        public Student Student { get; set; }
    }
}
