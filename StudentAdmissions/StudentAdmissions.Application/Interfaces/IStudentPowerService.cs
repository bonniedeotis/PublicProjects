using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IStudentPowerService
    {
        Result<List<StudentPower>> GetStudentPowers(int studentId);
        Result<StudentPower> GetStudentPower(int studentId, int powerId);
        Result<StudentPower> AddPowerToStudent(StudentPower model);
        Result DeleteStudentPower(StudentPower model);
        Result EditStudentPower(StudentPower model);

    }
}
