using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class PowerService : IPowerService
    {
        private AcademyContext _context;

        public PowerService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<StudentsWithPower> GetStudentsWithPower(int powerId)
        {
            var studentList = new StudentsWithPower();

            try
            {
                var power = _context.Power.FirstOrDefault(p => p.PowerID == powerId);

                if (power != null)
                {
                    var list = _context.StudentPower
                        .Where(sp => sp.PowerID == powerId)
                        .Include(sp => sp.Student)
                        .Include(sp => sp.Power)
                            .ThenInclude(p => p.PowerType)
                        .ToList();

                    studentList.StudentPowers = list;
                    studentList.PowerName = power.PowerName;
                }
                return ResultFactory.Success(studentList);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentsWithPower>(ex.Message);
            }
        }

        public Result<List<Power>> GetAllPowers()
        {
            List<Power> powers = new List<Power>();

            try
            {
                powers = _context.Power
                    .Include(p => p.PowerType)
                    .ToList();

                if (powers.Count() == 0)
                {
                    return ResultFactory.Fail<List<Power>>("Error getting power list");
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Power>>(ex.Message);
            }

            return ResultFactory.Success(powers);
        }

        public Result<Power> GetPower(int powerId)
        {
            var power = new Power();

            try
            {
                power = _context.Power
                    .FirstOrDefault(w => w.PowerID == powerId);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Power>("Error getting power data");
            }

            return ResultFactory.Success(power);
        }
    }
}
