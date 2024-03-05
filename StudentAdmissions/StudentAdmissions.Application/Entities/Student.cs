namespace StudentAdmissions.Application.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public DateOnly? DoB { get; set; }

        public List<StudentPower> Powers { get; set; }
        public List<StudentWeakness> Weaknesses { get; set; }
        public List<StudentSection> StudentSections { get; set; }
    }
}
