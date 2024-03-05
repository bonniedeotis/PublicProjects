using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IPowerService
    {
        Result<StudentsWithPower> GetStudentsWithPower(int powerId);
        Result<List<Power>> GetAllPowers();
        Result<Power> GetPower(int id);

    }
}
