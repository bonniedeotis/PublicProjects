using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IStudentWeaknessService
    {

        Result<List<StudentWeakness>> GetStudentWeaknesses(int studentId);
        Result<StudentWeakness> AddWeaknessToStudent(StudentWeakness model); //add weakness to student
        Result DeleteStudentWeakness(StudentWeakness model); //remove weakness from student
        Result<StudentWeakness> GetStudentWeakness(int studentId, int weaknessId);
        Result EditStudentWeakness(StudentWeakness model); //edit existing student weakness

    }
}
