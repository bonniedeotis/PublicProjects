using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.StudentPowerModels
{
    public class AddPowerToStudentViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int PowerID { get; set; }
        public byte Rating { get; set; }
        public List<Power> Powers { get; set; }
    }
}
