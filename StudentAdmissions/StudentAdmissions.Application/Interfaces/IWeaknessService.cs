using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IWeaknessService
    {
        Result<List<Weakness>> GetAllWeaknesses();
        Result<Weakness> GetWeakness(int weaknessId);
        Result<StudentsWithWeakness> GetStudentsWithWeakness(int weaknessId);
    }
}
