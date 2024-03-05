using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.DTOs
{
    public class StudentsWithPower
    {
        public string PowerName { get; set; }
        public List<StudentPower> StudentPowers { get; set; }
    }
}
