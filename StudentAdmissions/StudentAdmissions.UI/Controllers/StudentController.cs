using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.CourseModels;
using StudentAdmissions.UI.Models.StudentModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    public class StudentController : Controller
    {
        private IStudentService _studentService;
        private ISectionService _sectionService;

        public StudentController(IStudentService studentService, ISectionService sectionService)
        {
            _studentService = studentService;
            _sectionService = sectionService;
        }

        [HttpGet]
        [Route("students")]
        public IActionResult ListStudents()
        {
            var result = _studentService.GetAllStudents();

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("student/{id}")]
        [HttpGet]
        public IActionResult GetStudent(int id)
        {
            var model = new GetStudentViewModel();

            var studentResult = _studentService.GetStudentProfile(id);

            if (studentResult.Ok)
            {
                model.Student = studentResult.Data.Student;
                model.StudentPowers = studentResult.Data.StudentPowers;
                if (model.StudentPowers.Count == 0)
                {
                    model.PowerMessage = "No powers are currently assigned";
                }
                model.StudentWeaknesses = studentResult.Data.StudentWeaknesses;
                if (model.StudentWeaknesses.Count == 0)
                {
                    model.WeaknessMessage = "No weaknesses are currently assigned";
                }
                model.Sections = studentResult.Data.Sections;
                if (model.Sections.Count == 0)
                {
                    model.SectionMessage = "No sections are currently assigned";
                }
                return View(model);
            }
            else
            {
                return StatusCode(500, "Error getting student information");
            }
        }

        [Route("student/add")]
        [HttpGet]
        public IActionResult AddStudent()
        {
            // we want to bind @model, so we will send an empty object
            var model = new AddOrUpdateStudent();
            return View(model);
        }

        [Route("student/add")]
        [HttpPost]
        public IActionResult AddStudent(AddOrUpdateStudent model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student();

                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Alias = model.Alias;
                student.DoB = model.DoB;

                _studentService.Add(student);

                return RedirectToAction("ListStudents");
            }
            else
            {
                return StatusCode(500, "Error creating new student record");
            }
        }

        [Route("student/{id}/edit")]
        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            var result = _studentService.GetStudentById(id);

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("student/{id}/edit")]
        [HttpPost]
        public IActionResult EditStudent(Student model)
        {
            var result = _studentService.Edit(model);

            if (result.Ok)
            {
                return RedirectToAction("ListStudents");
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("student/{id}/remove")]
        [HttpGet]
        public IActionResult DeleteStudent(int id)
        {
            var result = _studentService.GetStudentById(id);

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("student/{id}/remove")]
        [HttpPost]
        public IActionResult DeleteStudent(Student model)
        {
            var result = _studentService.Delete(model);

            if (result.Ok)
            {
                return RedirectToAction("ListStudents");
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("student/enroll")]
        [HttpGet]
        public IActionResult EnrollStudent()
        {
            var model = new AddOrUpdateStudent();

            return View(model);
        }

        [Route("student/enroll")]
        [HttpPost]
        public IActionResult EnrollStudent(AddOrUpdateStudent model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student();

                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Alias = model.Alias;
                student.DoB = model.DoB;

                var result = _studentService.Add(student);

                if (result.Ok)
                {
                    return RedirectToAction("WelcomeNewStudent", new { id = result.Data.StudentID });
                }
                else
                {
                    return StatusCode(400, "Something went wrong enrolling the student.");
                }
            }
            else
            {
                return StatusCode(500, "Error enrolling new student");
            }
        }

        [Route("student/{id}/welcome")]
        [HttpGet]
        public IActionResult WelcomeNewStudent(int id)
        {
            var model = new Student();
            var studentResult = _studentService.GetStudentById(id);

            if (studentResult.Ok)
            {
                model = studentResult.Data;

                return View(model);
            }
            else
            {
                return StatusCode(500, studentResult.Message);
            }

        }

        [Route("student/{id}/section/open")]
        [HttpGet]
        public IActionResult GetOpenSections(int id)
        {
            var model = new OpenSectionsViewModel();

            var studentResult = _studentService.GetStudentById(id);
            var openSections = _sectionService.GetOpenSections();

            if (studentResult.Ok && openSections.Ok)
            {
                model.StudentID = id;
                model.StudentName = studentResult.Data.FirstName + " " + studentResult.Data.LastName;
                model.OpenSections = openSections.Data;

                return View(model);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {openSections.Message}");
            }
        }
    }
}
