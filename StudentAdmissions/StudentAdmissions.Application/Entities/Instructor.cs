namespace StudentAdmissions.Application.Entities
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string Alias { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? TermDate { get; set; }

        public List<Section> Sections { get; set; }
    }
}
