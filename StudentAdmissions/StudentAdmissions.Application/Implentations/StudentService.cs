using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class StudentService : IStudentService
    {
        private AcademyContext _context;

        public StudentService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<Student> Add(Student model)
        {
            if (model == null)
            {
                return ResultFactory.Fail<Student>("Error adding student.");
            }

            try
            {
                _context.Student.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Student>(ex.Message);
            }

            return ResultFactory.Success(model);
        }

        public Result Edit(Student model)
        {
            try
            {
                _context.Student.Update(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }

            return ResultFactory.Success();
        }

        public Result<List<Student>> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            try
            {
                students = _context.Student.ToList();

                if (students.Count() == 0)
                {
                    return ResultFactory.Fail<List<Student>>("Error getting student list");
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Student>>(ex.Message);
            }

            return ResultFactory.Success(students);
        }

        public Result<Student> GetStudentById(int studentId)
        {
            Student student = new Student();

            try
            {
                student = _context.Student.FirstOrDefault(s => s.StudentID == studentId);

                if (student == null)
                {
                    return ResultFactory.Fail<Student>("Student not found");
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Student>(ex.Message);
            }

            return ResultFactory.Success(student);

        }

        public Result Delete(Student model)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var sections = _context.StudentSection.Where(s => s.StudentID == model.StudentID);
                    _context.StudentSection.RemoveRange(sections);

                    var powers = _context.StudentPower.Where(sp => sp.StudentID == model.StudentID);
                    _context.StudentPower.RemoveRange(powers);

                    var weaknesses = _context.StudentWeakness.Where(sw => sw.StudentID == model.StudentID);
                    _context.StudentWeakness.RemoveRange(weaknesses);

                    var student = _context.Student.FirstOrDefault(s => s.StudentID == model.StudentID);
                    _context.Student.Remove(student);

                    _context.SaveChanges();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }

            return ResultFactory.Success();
        }

        public Result<StudentProfile> GetStudentProfile(int studentId)
        {
            StudentProfile profile = new StudentProfile();

            try
            {
                profile.Student = _context.Student.FirstOrDefault(s => s.StudentID == studentId);

                if (profile.Student == null)
                {
                    return ResultFactory.Fail<StudentProfile>("Student not found");
                }

                profile.StudentPowers = _context.StudentPower
                    .Where(sp => sp.StudentID == studentId)
                    .Include(sp => sp.Power)
                        .ThenInclude(p => p.PowerType)
                    .ToList();

                profile.StudentWeaknesses = _context.StudentWeakness
                    .Where(sw => sw.StudentID == studentId)
                    .Include(sw => sw.Weakness)
                        .ThenInclude(w => w.WeaknessType)
                    .ToList();

                profile.Sections = _context.StudentSection
                    .Where(ss => ss.StudentID == studentId)
                    .Include(ss => ss.Section.Course)
                    .Include(ss => ss.Section)
                        .ThenInclude(s => s.Instructor)
                    .ToList();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentProfile>(ex.Message);
            }

            return ResultFactory.Success(profile);
        }
    }
}
