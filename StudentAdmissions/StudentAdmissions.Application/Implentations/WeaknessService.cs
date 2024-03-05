using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class WeaknessService : IWeaknessService
    {
        private AcademyContext _context;

        public WeaknessService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<List<Weakness>> GetAllWeaknesses()
        {
            List<Weakness> weaknesses = new List<Weakness>();

            try
            {
                weaknesses = _context.Weakness
                    .Include(w => w.WeaknessType)
                    .ToList();

                if (weaknesses.Count() == 0)
                {
                    return ResultFactory.Fail<List<Weakness>>("Error getting weakness list");
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Weakness>>(ex.Message);
            }

            return ResultFactory.Success(weaknesses);
        }

        public Result<StudentsWithWeakness> GetStudentsWithWeakness(int weaknessId)
        {
            var studentList = new StudentsWithWeakness();

            try
            {
                var weakness = _context.Weakness.FirstOrDefault(w => w.WeaknessID == weaknessId);

                if (weakness != null)
                {
                    var list = _context.StudentWeakness
                        .Where(sw => sw.WeaknessID == weaknessId)
                        .Include(sw => sw.Student)
                        .Include(sw => sw.Weakness)
                            .ThenInclude(w => w.WeaknessType)
                        .ToList();

                    studentList.Students = list;
                    studentList.WeaknessName = weakness.WeaknessName;
                }
                return ResultFactory.Success(studentList);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentsWithWeakness>(ex.Message);
            }

        }

        public Result<Weakness> GetWeakness(int weaknessId)
        {
            var weakness = new Weakness();

            try
            {
                weakness = _context.Weakness
                    .FirstOrDefault(w => w.WeaknessID == weaknessId);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Weakness>("Error getting weakness data");
            }

            return ResultFactory.Success(weakness);
        }
    }
}
