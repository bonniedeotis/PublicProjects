using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentAdmissions.UI.Models.StudentSectionModels
{
    public class AddStudentToSectionViewModel
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        public int SectionID { get; set; }


        public List<Student>? Students { get; set; }
        public SectionInfo? SectionInfo { get; set; }
        public int? CourseID { get; set; }
        public string? CourseName { get; set; }
    }
}
