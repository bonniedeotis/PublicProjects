using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IStudentService
    {
        Result<List<Student>> GetAllStudents();
        Result<Student> GetStudentById(int studentId);
        Result<Student> Add(Student model);
        Result Edit(Student model);
        Result Delete(Student model);
        Result<StudentProfile> GetStudentProfile(int studentId);
    }
}
