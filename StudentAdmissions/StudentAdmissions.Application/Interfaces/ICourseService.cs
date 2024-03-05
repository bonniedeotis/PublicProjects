using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface ICourseService
    {
        Result<List<Course>> GetCourses();
        Result<Course> AddCourse(Course model);
        Result EditCourse(Course model);
        Result<List<Subject>> GetSubjects();
        Result<Course> GetCourse(int id);

    }
}
