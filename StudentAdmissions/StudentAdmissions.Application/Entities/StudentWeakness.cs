using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Entities
{
    [PrimaryKey(nameof(StudentID), nameof(WeaknessID))]
    public class StudentWeakness
    {
        public int StudentID { get; set; }
        public int WeaknessID { get; set; }
        public byte RiskLevel { get; set; }

        public Student Student { get; set; } = null;
        public Weakness Weakness { get; set; } = null;
    }
}
