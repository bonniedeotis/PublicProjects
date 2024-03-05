using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.Application.Interfaces
{
    public interface ISectionService
    {
        Result AddSection(Section model);
        Result<List<Section>> GetOpenSections();
        Result<List<Section>> GetCourseSections(int id);
    }
}
