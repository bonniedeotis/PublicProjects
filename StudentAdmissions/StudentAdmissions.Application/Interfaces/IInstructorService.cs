using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IInstructorService
    {
        Result<List<Instructor>> GetInstructors();
    }
}
