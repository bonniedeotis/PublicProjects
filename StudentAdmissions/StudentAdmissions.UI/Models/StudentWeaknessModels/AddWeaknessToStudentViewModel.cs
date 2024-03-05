using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.StudentWeaknessModels
{
    public class AddWeaknessToStudentViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int WeaknessID { get; set; }
        public byte RiskLevel { get; set; }
        public List<Weakness> Weaknesses { get; set; }
    }
}
