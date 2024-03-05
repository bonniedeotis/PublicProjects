using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.StudentModels
{
    public class NewStudentEnrollment
    {
        public AddOrUpdateStudent StudentInfo { get; set; }
        public List<Weakness> WeaknessNames { get; set; }
        public List<Power> PowerNames { get; set; }
        public List<Weakness> AddWeaknesses { get; set; }
        public List<Power> AddPowers { get; set; }
    }
}
