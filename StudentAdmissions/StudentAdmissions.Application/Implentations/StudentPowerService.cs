using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class StudentPowerService : IStudentPowerService
    {
        private AcademyContext _context;

        public StudentPowerService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }
        public Result<StudentPower> AddPowerToStudent(StudentPower model)
        {
            if (model == null)
            {
                return ResultFactory.Fail<StudentPower>($"Error adding power to student ID {model.StudentID}.");
            }

            var alreadyHasPower = _context.StudentPower.Any(sw => sw.StudentID == model.StudentID && sw.PowerID == model.PowerID);

            if (alreadyHasPower)
            {
                return ResultFactory.Fail<StudentPower>($"Student ID {model.StudentID} already has that power assigned to them.");
            }

            try
            {
                _context.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentPower>(ex.Message);
            }

            return ResultFactory.Success(model);
        }

        public Result DeleteStudentPower(StudentPower model)
        {
            if (model == null)
            {
                return ResultFactory.Fail($"Error removing power from student ID {model.StudentID}");
            }

            var hasPower = _context.StudentPower.Any(sw => sw.StudentID == model.StudentID && sw.PowerID == model.PowerID);

            if (hasPower)
            {
                try
                {
                    _context.Remove(model);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ResultFactory.Fail(ex.Message);
                }
            }

            return ResultFactory.Success();
        }

        public Result EditStudentPower(StudentPower model)
        {
            try
            {
                _context.StudentPower.Update(model);
                _context.SaveChanges();

                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<StudentPower> GetStudentPower(int studentId, int powerId)
        {
            try
            {
                var power = new StudentPower();

                power = _context.StudentPower
                    .Include(sp => sp.Student)
                    .Include(sp => sp.Power)
                    .FirstOrDefault(sw => sw.StudentID == studentId && sw.PowerID == powerId);

                if (power != null)
                {
                    return ResultFactory.Success(power);
                }
                return ResultFactory.Fail<StudentPower>("Error getting power data");
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentPower>(ex.Message);
            }
        }

        public Result<List<StudentPower>> GetStudentPowers(int studentId)
        {
            List<StudentPower> list = new List<StudentPower>();

            try
            {
                list = _context.StudentPower
                    .Where(sw => sw.StudentID == studentId)
                    .Include(sw => sw.Student)
                    .Include(sw => sw.Power)
                        .ThenInclude(w => w.PowerType)
                    //.OrderBy(sw => sw.Rating)
                    .ToList();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<StudentPower>>("Error getting list");
            }

            return ResultFactory.Success(list);
        }




    }
}
