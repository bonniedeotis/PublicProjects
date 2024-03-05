using StudentAdmissions.Application.Interfaces;
using StudentAdmissions.UI.Models.WeaknessModels;
using Microsoft.AspNetCore.Mvc;

namespace StudentAdmissions.UI.Controllers
{
    [Route("weakness")]
    public class WeaknessController : Controller
    {
        private IWeaknessService _weaknessService;
        private IStudentWeaknessService _studentWeaknessService;

        public WeaknessController(IWeaknessService weaknessService, IStudentWeaknessService studentWeaknessService)
        {
            _weaknessService = weaknessService;
            _studentWeaknessService = studentWeaknessService;
        }

        [HttpGet]
        public IActionResult GetWeaknesses()
        {
            var model = new ListWeaknessesViewModel();
            var result = _weaknessService.GetAllWeaknesses();

            if (result.Ok)
            {
                model.Weaknesses = result.Data;
                return View(model);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        [HttpGet]
        [Route("{weaknessId}")]
        public IActionResult GetStudentsWithWeakness(int weaknessId)
        {
            var model = _weaknessService.GetStudentsWithWeakness(weaknessId);

            if (model.Ok)
            {
                return View(model.Data);
            }
            else
            {
                throw new Exception(model.Message);
            }
        }


    }
}
