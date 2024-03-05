using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.DTOs
{
    public class StudentsInSection
    {
        public List<StudentSection> Students { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string SubjectName { get; set; }
        public Section Section { get; set; }
        public string InstructorAlias { get; set; }
        public Enrollment EnrollmentStatus { get; set; }
    }

    public enum Enrollment
    {
        Open,
        Closed
    }
}
