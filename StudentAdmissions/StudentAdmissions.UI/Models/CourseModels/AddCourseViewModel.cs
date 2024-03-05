using StudentAdmissions.Application.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentAdmissions.UI.Models.CourseModels
{
    public class AddCourseViewModel
    {
        [Required]
        public int SubjectID { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string CourseDescription { get; set; }

        [Required]
        public decimal Credits { get; set; }


        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
