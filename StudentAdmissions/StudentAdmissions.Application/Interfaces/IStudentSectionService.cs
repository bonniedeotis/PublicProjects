using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface IStudentSectionService
    {
        Result<StudentsInSection> GetStudentsInSection(int sectionId);
        Result<List<StudentSection>> GetStudentSections(int studentId);
        Result<StudentSection> AddStudentToSection(StudentSection model);
        Result<SectionInfo> GetSection(int sectionId);
    }
}
