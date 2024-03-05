using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Entities
{
    [PrimaryKey(nameof(StudentID), nameof(PowerID))]

    public class StudentPower
    {
        public int StudentID { get; set; }
        public int PowerID { get; set; }
        public byte Rating { get; set; }

        public Student Student { get; set; }
        public Power Power { get; set; }
    }
}
