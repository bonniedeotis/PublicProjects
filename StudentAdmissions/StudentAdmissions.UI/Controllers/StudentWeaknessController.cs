using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.StudentWeaknessModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    [Route("student")]
    public class StudentWeaknessController : Controller
    {
        private IStudentService _studentService;
        private IStudentWeaknessService _studentWeaknessService;
        private IWeaknessService _weaknessService;

        public StudentWeaknessController(IStudentService studentService, IStudentWeaknessService studentWeaknessService, IWeaknessService weaknessService)
        {
            _studentService = studentService;
            _studentWeaknessService = studentWeaknessService;
            _weaknessService = weaknessService;
        }

        [Route("{id}/weaknesses/add")]
        [HttpGet]
        public IActionResult AddStudentWeakness(int id)
        {
            var model = new AddWeaknessToStudentViewModel();

            var sResult = _studentService.GetStudentById(id);
            var wResult = _weaknessService.GetAllWeaknesses();

            if (sResult.Ok && wResult.Ok)
            {
                // we want to bind @model, so we will send an empty object
                model.StudentID = id;
                model.StudentName = sResult.Data.FirstName + " " + sResult.Data.LastName;
                model.Weaknesses = wResult.Data;

                return View(model);
            }

            else
            {
                throw new Exception($"Error loading add weaknesses.");
            }
        }

        [Route("{id}/weaknesses/add")]
        [HttpPost]
        public IActionResult AddStudentWeakness(AddWeaknessToStudentViewModel model)
        {
            var newWeakness = new StudentWeakness();

            var sResult = _studentService.GetStudentById(model.StudentID);
            var wResult = _weaknessService.GetWeakness(model.WeaknessID);

            if (sResult.Ok && wResult.Ok)
            {
                newWeakness.StudentID = model.StudentID;
                newWeakness.WeaknessID = model.WeaknessID;
                newWeakness.RiskLevel = model.RiskLevel;
            }

            var result = _studentWeaknessService.AddWeaknessToStudent(newWeakness);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("{id}/weaknesses/remove/{weaknessId}")]
        [HttpGet]
        public IActionResult DeleteStudentWeakness(int id, int weaknessId)
        {
            var result = _studentWeaknessService.GetStudentWeaknesses(id);

            foreach (var weakness in result.Data)
            {
                if (weakness.WeaknessID == weaknessId)
                {
                    var model = new StudentWeakness();

                    model.Student = weakness.Student;
                    model.StudentID = weakness.StudentID;
                    model.Weakness = weakness.Weakness;
                    model.WeaknessID = weakness.WeaknessID;
                    model.RiskLevel = weakness.RiskLevel;

                    return View(model);
                }
            }

            return StatusCode(500, $"Internal Error: {result.Message}");
        }

        [Route("{id}/weaknesses/remove/{weaknessId}")]
        [HttpPost]
        public IActionResult DeleteStudentWeakness(StudentWeakness model)
        {
            var result = _studentWeaknessService.DeleteStudentWeakness(model);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("{id}/weaknesses/edit/{weaknessId}")]
        [HttpGet]
        public IActionResult EditStudentWeakness(int id, int weaknessId)
        {
            var result = _studentWeaknessService.GetStudentWeakness(id, weaknessId);

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }

        [Route("{id}/weaknesses/edit/{weaknessId}")]
        [HttpPost]
        public IActionResult EditStudentWeakness(StudentWeakness model)
        {
            var result = _studentWeaknessService.EditStudentWeakness(model);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }
    }
}
