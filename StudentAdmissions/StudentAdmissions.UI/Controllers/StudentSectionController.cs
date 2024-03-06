using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.StudentSectionModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    public class StudentSectionController : Controller
    {
        private IStudentSectionService _studentSectionService;
        private IStudentService _studentService;
        private ICourseService _courseService;

        public StudentSectionController(IStudentSectionService studentSectionService, IStudentService studentService, ICourseService courseService)
        {
            _studentSectionService = studentSectionService;
            _studentService = studentService;
            _courseService = courseService;
        }

        [Route("course/{courseId}/section/{sectionId}")]
        [HttpGet]
        public IActionResult GetStudentsInSection(int sectionId)
        {
            var result = _studentSectionService.GetStudentsInSection(sectionId);

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("course/{courseId}/section/{sectionId}/add/{studentId}")]
        [HttpGet]
        public IActionResult AddStudentToSection(int sectionId, int courseId, int? studentId)
        {
            var model = new AddStudentToSectionViewModel();


            var sectionResult = _studentSectionService.GetSection(sectionId);

            if (sectionResult.Data.EndDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return StatusCode(400, $"Section ID {sectionId} is closed for enrollment.");
            }

            if (studentId != null)
            {
                var studentResult = _studentService.GetStudentById((int)studentId);

                if (studentResult.Ok)
                {
                    model.Students = new List<Student> { studentResult.Data };
                }
                else
                {
                    return StatusCode(500, $"Internal Error: {studentResult.Message}");
                }
            }

            var courseResult = _courseService.GetCourse(courseId);

            if (courseResult.Ok)
            {
                model.SectionID = sectionId;
                model.SectionInfo = sectionResult.Data;
                model.CourseID = courseResult.Data.CourseID;
                model.CourseName = courseResult.Data.CourseName;
            }
            else
            {
                return StatusCode(500, $"Internal Error: {courseResult.Message}");
            }

            return View(model);
        }

        [Route("course/{courseId}/section/{sectionId}/add/{studentId}")]
        [HttpPost]
        public IActionResult AddStudentToSection(AddStudentToSectionViewModel model, int courseId, int sectionId)
        {
            if (ModelState.IsValid)
            {
                var studentSection = new StudentSection();

                studentSection.StudentID = model.StudentID;
                studentSection.SectionID = model.SectionID;

                var result = _studentSectionService.AddStudentToSection(studentSection);

                if (result.Ok)
                {
                    return RedirectToAction("GetStudent", "Student", new { id = result.Data.StudentID });
                }
                else
                {
                    return StatusCode(500, $"Internal Error: {result.Message}");
                }
            }
            else
            {
                return StatusCode(500, "Error enrolling student in section");
            }
        }
    }
}
