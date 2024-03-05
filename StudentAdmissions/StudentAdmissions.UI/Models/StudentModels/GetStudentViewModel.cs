using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.StudentModels
{
    public class GetStudentViewModel
    {
        public Student Student { get; set; }
        public List<StudentPower> StudentPowers { get; set; }
        public List<StudentWeakness> StudentWeaknesses { get; set; }
        public List<StudentSection> Sections { get; set; }
        public string PowerMessage { get; set; } = "";
        public string WeaknessMessage { get; set; } = "";
        public string SectionMessage { get; set; } = "";
    }
}
