using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.StudentPowerModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    [Route("student")]
    public class StudentPowerController : Controller
    {
        private IStudentService _studentService;
        private IStudentPowerService _studentPowerService;
        private IPowerService _powerService;

        public StudentPowerController(IStudentService studentService, IStudentPowerService studentPowerService,
            IPowerService powerService)
        {
            _studentService = studentService;
            _studentPowerService = studentPowerService;
            _powerService = powerService;
        }

        [Route("{id}/powers/add")]
        [HttpGet]
        public IActionResult AddStudentPower(int id)
        {
            var model = new AddPowerToStudentViewModel();

            var sResult = _studentService.GetStudentById(id);
            var pResult = _powerService.GetAllPowers();

            if (sResult.Ok && pResult.Ok)
            {
                // we want to bind @model, so we will send an empty object
                model.StudentID = id;
                model.StudentName = sResult.Data.FirstName + " " + sResult.Data.LastName;
                model.Powers = pResult.Data;

                return View(model);
            }

            else
            {
                throw new Exception($"Error loading add powers.");
            }
        }

        [Route("{id}/powers/add")]
        [HttpPost]
        public IActionResult AddStudentPower(AddPowerToStudentViewModel model)
        {
            var newPower = new StudentPower();

            var sResult = _studentService.GetStudentById(model.StudentID);
            var wResult = _powerService.GetPower(model.PowerID);

            if (sResult.Ok && wResult.Ok)
            {
                newPower.StudentID = model.StudentID;
                newPower.PowerID = model.PowerID;
                newPower.Rating = model.Rating;
            }

            var result = _studentPowerService.AddPowerToStudent(newPower);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                // todo: add validation messages to form later
                throw new Exception(result.Message);
            }
        }

        [Route("{id}/powers/remove/{powerId}")]
        [HttpGet]
        public IActionResult DeleteStudentPower(int id, int powerId)
        {
            var result = _studentPowerService.GetStudentPowers(id);

            foreach (var power in result.Data)
            {
                if (power.PowerID == powerId)
                {
                    var model = new StudentPower();

                    model.Student = power.Student;
                    model.StudentID = power.StudentID;
                    model.Power = power.Power;
                    model.PowerID = power.PowerID;
                    model.Rating = power.Rating;

                    return View(model);
                }
            }

            throw new Exception(result.Message);
        }

        [Route("{id}/powers/remove/{powerId}")]
        [HttpPost]
        public IActionResult DeleteStudentPower(StudentPower model)
        {
            var result = _studentPowerService.DeleteStudentPower(model);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                // todo: add validation messages to form later
                throw new Exception(result.Message);
            }
        }

        [Route("{id}/powers/edit/{powerId}")]
        [HttpGet]
        public IActionResult EditStudentPower(int id, int powerId)
        {
            var result = _studentPowerService.GetStudentPower(id, powerId);

            if (result.Ok)
            {
                return View(result.Data);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [Route("{id}/powers/edit/{powerId}")]
        [HttpPost]
        public IActionResult EditStudentPower(StudentPower model)
        {
            var result = _studentPowerService.EditStudentPower(model);

            if (result.Ok)
            {
                return RedirectToAction("GetStudent", "Student", new { id = model.StudentID });
            }
            else
            {
                // todo: add validation messages to form later
                throw new Exception(result.Message);
            }
        }
    }
}
