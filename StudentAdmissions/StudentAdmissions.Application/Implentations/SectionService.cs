using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class SectionService : ISectionService
    {
        private AcademyContext _context;

        public SectionService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result AddSection(Section model)
        {
            if (model == null)
            {
                return ResultFactory.Fail($"Error adding new section");
            }

            try
            {
                _context.Section.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }

            return ResultFactory.Success();
        }

        public Result<List<Section>> GetOpenSections()
        {
            try
            {
                var list = new List<Section>();

                list = _context.Section
                    .Where(s => s.EndDate >= DateOnly.FromDateTime(DateTime.Today))
                    .Include(s => s.Course)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Instructor)
                    .ToList();

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Section>>(ex.Message);
            }
        }

        public Result<List<Section>> GetCourseSections(int courseId)
        {
            try
            {
                var list = new List<Section>();

                list = _context.Section
                    .Where(s => s.CourseID == courseId)
                    .Include(s => s.Course)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Instructor)
                    .ToList();

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Section>>(ex.Message);
            }
        }
    }
}
