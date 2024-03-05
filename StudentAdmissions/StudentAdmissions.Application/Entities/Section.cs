namespace StudentAdmissions.Application.Entities
{
    public class Section
    {
        public int SectionID { get; set; }
        public int CourseID { get; set; }
        public int InstructorID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
    }
}
