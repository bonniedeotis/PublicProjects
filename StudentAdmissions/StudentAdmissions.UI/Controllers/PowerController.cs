using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.PowerModels;
using Microsoft.AspNetCore.Mvc;
using StudentAdmissions.Application.DTOs;

namespace StudentAdmissions.UI.Controllers
{
    [Route("power")]
    public class PowerController : Controller
    {
        private IPowerService _powerService;
        private IStudentPowerService _studentPowerService;

        public PowerController(IPowerService powerService, IStudentPowerService studentService)
        {
            _powerService = powerService;
            _studentPowerService = studentService;
        }

        [HttpGet]
        [Route("{powerId}")]
        public IActionResult GetStudentsWithPower(int powerId)
        {
            var model = _powerService.GetStudentsWithPower(powerId);

            if (model.Ok)
            {
                return View(model.Data);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {model.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetPowers()
        {
            var model = new ListPowersViewModel();
            var result = _powerService.GetAllPowers();

            if (result.Ok)
            {
                model.Powers = result.Data;
                return View(model);
            }
            else
            {
                return StatusCode(500, $"Internal Error: {result.Message}");
            }
        }
    }
}
