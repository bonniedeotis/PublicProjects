using StudentAdmissions.Application.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentAdmissions.UI.Models.CourseModels
{
    public class AddSectionViewModel
    {
        [Required]
        public int CourseID { get; set; }

        [Required]
        public int InstructorID { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
}
