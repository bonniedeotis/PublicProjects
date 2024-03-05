using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.CourseModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IInstructorService _instructorService;
        private ISectionService _sectionService;

        public CourseController(ICourseService courseService, IInstructorService instructorService,
            ISectionService sectionService)
        {
            _courseService = courseService;
            _instructorService = instructorService;
            _sectionService = sectionService;
        }

        [Route("courses")]
        [HttpGet]
        public IActionResult GetCourses()
        {
            var result = _courseService.GetCourses();

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }



        [Route("course/add")]
        [HttpGet]
        public IActionResult AddCourse(int id)
        {
            var model = new AddCourseViewModel();
            var result = _courseService.GetSubjects();

            if (result.Ok)
            {
                model.Subjects = result.Data;
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }

            return View(model);
        }

        [Route("course/add")]
        [HttpPost]
        public IActionResult AddCourse(AddCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course();

                course.SubjectID = model.SubjectID;
                course.CourseName = model.CourseName;
                course.CourseDescription = model.CourseDescription;
                course.Credits = model.Credits;

                var result = _courseService.AddCourse(course);

                if (result.Ok)
                {
                    return RedirectToAction("GetCourses");
                }
                else
                {
                    return StatusCode(500, $"Internal Error: {result.Message}");
                }
            }
            else
            {
                throw new Exception("Error creating new course");
            }
        }

        [Route("course/{id}/section/add")]
        [HttpGet]
        public IActionResult AddSection(int id)
        {
            var model = new AddSectionViewModel();

            var courseResult = _courseService.GetCourses();
            var instructorResult = _instructorService.GetInstructors();

            if (courseResult.Ok && instructorResult.Ok)
            {
                model.CourseID = id;
                model.Courses = courseResult.Data;
                model.Instructors = instructorResult.Data;
            }
            else
            {
                return StatusCode(500, $"Internal Error: {courseResult.Message} {instructorResult.Message}");
            }

            return View(model);
        }

        [Route("course/{id}/section/add")]
        [HttpPost]
        public IActionResult AddSection(AddSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var section = new Section();

                section.CourseID = model.CourseID;
                section.InstructorID = model.InstructorID;
                section.StartDate = model.StartDate;
                section.EndDate = model.EndDate;
                section.StartTime = model.StartTime;
                section.EndTime = model.EndTime;

                var result = _sectionService.AddSection(section);

                if (result.Ok)
                {
                    return RedirectToAction("GetCourseSections", new { id = model.CourseID });
                }
                else
                {
                    return StatusCode(500, $"Internal Error: {result.Message}");
                }
            }
            else
            {
                throw new Exception("Error creating new section");
            }
        }

        [Route("course/{id}/sections")]
        [HttpGet]
        public IActionResult GetCourseSections(int id)
        {
            var model = new GetSectionsViewModel();

            var sResult = _sectionService.GetCourseSections(id);

            var cResult = _courseService.GetCourse(id);

            if (sResult.Ok && cResult.Ok)
            {
                model.Sections = sResult.Data;
                model.Course = cResult.Data;

                if (sResult.Data.Count == 0)
                {
                    model.Message = "No sections currently assigned";
                }
                return View(model);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {sResult.Message}");
            }
        }


    }
}
