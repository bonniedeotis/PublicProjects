using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class StudentWeaknessService : IStudentWeaknessService
    {
        private AcademyContext _context;

        public StudentWeaknessService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<Weakness> AddNewWeakness(Weakness model) //FINISH WRITING
        {
            Weakness weakness = new Weakness();

            if (model == null)
            {
                return ResultFactory.Fail<Weakness>("Error adding weakness.");
            }

            try
            {
                weakness.WeaknessName = model.WeaknessName;
                weakness.WeaknessDescription = model.WeaknessDescription;

                _context.Add(weakness);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Weakness>(ex.Message);
            }

            return ResultFactory.Success(weakness);
        }

        public Result<StudentWeakness> AddWeaknessToStudent(StudentWeakness model)
        {
            if (model == null)
            {
                return ResultFactory.Fail<StudentWeakness>($"Error adding weakness to student ID {model.StudentID}.");
            }

            var alreadyHasWeakness = _context.StudentWeakness.Any(sw => sw.StudentID == model.StudentID && sw.WeaknessID == model.WeaknessID);

            if (alreadyHasWeakness)
            {
                return ResultFactory.Fail<StudentWeakness>($"Student ID {model.StudentID} already has that weakness assigned to them.");
            }

            try
            {
                _context.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentWeakness>(ex.Message);
            }

            return ResultFactory.Success(model);
        }


        public Result DeleteStudentWeakness(StudentWeakness model)
        {
            if (model == null)
            {
                return ResultFactory.Fail($"Error removing weakness from student ID {model.StudentID}");
            }

            var hasWeakness = _context.StudentWeakness.Any(sw => sw.StudentID == model.StudentID && sw.WeaknessID == model.WeaknessID);

            if (hasWeakness)
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


        public Result EditStudentWeakness(StudentWeakness model)
        {
            try
            {
                _context.StudentWeakness.Update(model);
                _context.SaveChanges();

                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<StudentWeakness> GetStudentWeakness(int studentId, int weaknessId)
        {
            try
            {
                var weakness = new StudentWeakness();

                weakness = _context.StudentWeakness
                    .Include(sw => sw.Student)
                    .Include(sw => sw.Weakness)
                    .FirstOrDefault(sw => sw.StudentID == studentId && sw.WeaknessID == weaknessId);

                if (weakness != null)
                {
                    return ResultFactory.Success(weakness);
                }
                return ResultFactory.Fail<StudentWeakness>("Error getting weakness data");
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentWeakness>(ex.Message);
            }
        }

        public Result<List<StudentWeakness>> GetStudentWeaknesses(int studentId)
        {
            List<StudentWeakness> list = new List<StudentWeakness>();

            try
            {
                list = _context.StudentWeakness
                    .Where(sw => sw.StudentID == studentId)
                    .Include(sw => sw.Student)
                    .Include(sw => sw.Weakness)
                        .ThenInclude(w => w.WeaknessType)
                    //.OrderBy(sw => sw.RiskLevel)
                    .ToList();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<StudentWeakness>>("Error getting list");
            }

            return ResultFactory.Success(list);
        }
    }
}
