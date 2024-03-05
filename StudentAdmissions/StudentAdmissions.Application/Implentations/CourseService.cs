using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class CourseService : ICourseService
    {
        private AcademyContext _context;

        public CourseService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<Course> AddCourse(Course model)
        {
            if (model == null)
            {
                return ResultFactory.Fail<Course>($"Error adding new course");
            }

            try
            {
                _context.Course.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Course>(ex.Message);
            }

            return ResultFactory.Success(model);
        }

        public Result EditCourse(Course model)
        {
            try
            {
                _context.Course.Update(model);
                _context.SaveChanges();

                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<Course> GetCourse(int id)
        {
            try
            {
                var course = _context.Course
                    .Include(c => c.Sections)
                    .Include(c => c.Subject)
                    .FirstOrDefault(c => c.CourseID == id);

                if (course == null)
                {
                    return ResultFactory.Fail<Course>("Course not found");
                }
                return ResultFactory.Success(course);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Course>(ex.Message);
            }
        }

        public Result<List<Course>> GetCourses()
        {
            try
            {
                var list = new List<Course>();

                list = _context.Course
                    .Include(c => c.Subject)
                    .Include(c => c.Sections)
                        .ThenInclude(s => s.Instructor)
                    .ToList();

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Course>>(ex.Message);
            }
        }

        public Result<List<Subject>> GetSubjects()
        {
            try
            {
                var list = new List<Subject>();

                list = _context.Subject.ToList();

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Subject>>(ex.Message);
            }
        }
    }
}
